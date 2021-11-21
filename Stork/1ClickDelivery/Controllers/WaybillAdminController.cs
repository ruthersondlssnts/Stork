using _1ClickDelivery.Models;
using _1ClickDelivery.UserClasses;
using _1ClickDelivery.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace _1ClickDelivery.Controllers
{
    public class WaybillAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Admin")]
        // GET: WaybillAdmin
        public ActionResult Index()
        {
            var fromDate = TimeZoneHelper.GetTodayUTCPlus8String();
            var toDate = TimeZoneHelper.GetTodayUTCPlus8String();
            ViewBag.FromDate = fromDate;
            ViewBag.ToDate = toDate;


            var f = Convert.ToDateTime(fromDate);
            var t = Convert.ToDateTime(toDate);
            return View("Index", db.Waybills.Where(x => (x.DateOfPickup >= f && x.DateOfPickup <= t)).ToList());

            //return View(db.Waybills.ToList());
        }
        public ActionResult DetailModal(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waybill waybill = db.Waybills.Find(id);
            if (waybill == null)
            {
                return HttpNotFound();
            }
            return PartialView("_DetailsModalPartial", waybill);
        }



        [HttpPost]
        public ActionResult Refresh()
        {
            var from = Request["fromDate"].ToString();
            var to = Request["toDate"].ToString();
            var stat = Request["status"].ToString();

            ViewBag.FromDate = from;
            ViewBag.ToDate = to;
            ViewBag.Status = stat;

            var f = Convert.ToDateTime(from);
            var t = Convert.ToDateTime(to);

            if (stat == "All")
                return View("Index", db.Waybills.Where(x => (x.DateOfPickup >= f && x.DateOfPickup <= t)).ToList());
            else
                return View("Index", db.Waybills.Where(x => x.Status == stat && (x.DateOfPickup >= f && x.DateOfPickup <= t)).ToList());

        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Areas = new GetLists().GetAreas();
            var pickupDates = new GetLists().GetPickupDatesForAdmin();
            var wb = new AdminWaybillCreateViewModel() { PickupDates = pickupDates };
            return View(wb);
        }

        // POST: ADMIN Create Waybill
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Status,ManualWayBillNo,SenderName,SelectedDeliveryArea, SelectedArea,PickupAddress,ItemDescription, SpecialInstruction,DestinationAddress,ReceiverName,ReceiverPhoneNo, SelectedPickupDate")] AdminWaybillCreateViewModel waybill)
        {
            using (var db = new ApplicationDbContext())
            {
                if (ModelState.IsValid)
                {
                    var pkArea = new Guid(waybill.SelectedArea);
                    var a = db.Areas.Where(x => x.PKArea == pkArea).Single();
                    var area = a.AreaName;

                    var pkDeliveryArea = new Guid(waybill.SelectedDeliveryArea);
                    var b = db.Areas.Where(x => x.PKArea == pkDeliveryArea).Single();
                    var dAreaName = b.AreaName;

                    var senderId = User.Identity.GetUserId();
                    var uName = User.Identity.GetUserName();
                    var w = new Waybill()
                    {
                        PKWayBill = Guid.NewGuid(),
                        ManualWayBillNo = waybill.ManualWayBillNo,
                        PickupArea = area,
                        PickupAddress = waybill.PickupAddress,
                        DateOfPickup = waybill.SelectedPickupDate,
                        ItemDescription = waybill.ItemDescription,
                        SpecialInstruction = waybill.SpecialInstruction,
                        ReceiverName = waybill.ReceiverName,
                        ReceiverPhoneNo = waybill.ReceiverPhoneNo,
                        DeliveryArea = dAreaName,
                        DestinationAddress = waybill.DestinationAddress,
                        SenderId = senderId.ToString(),
                        SenderName = waybill.SenderName,
                        CreatedBy = uName,
                        Status = waybill.Status,
                        DateTimeCreated = TimeZoneHelper.GetTodayWithTimeUTCPlus8()
                    };
                    db.Waybills.Add(w);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                ViewBag.Areas = new GetLists().GetAreas();
                var pickupDates = new GetLists().GetPickupDates();
                var wb = new AdminWaybillCreateViewModel() { PickupDates = pickupDates };
                return View("Create", wb);
            }
        }


        // GET: Waybill/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Waybill waybill = db.Waybills.Find(id);
            if (waybill == null)
            {
                return HttpNotFound();
            }
            return View(waybill);
        }

        // POST: Waybill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "PKWayBill,WayBillNo,SenderName,PickupAddress,SpecialInstruction,DestinationAddress,ReceiverName,Status,DateOfPickup,DateTimeCreated,DateTimeUpdated")] Waybill waybill)
        public ActionResult Edit([Bind(Include = "PKWayBill,ManualWayBillNo,SenderName")] Waybill waybill)
        {
            Waybill wb = null;
            try
            {

                wb = db.Waybills.Where(x => x.PKWayBill == waybill.PKWayBill).Single();
                wb.ManualWayBillNo = waybill.ManualWayBillNo;
                //if (ModelState.IsValid)
                //{
                db.Entry(wb).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
                //}
            }
            catch
            {

                return View(wb);
            }

        }

        public FileResult PrintInlineWaybills(string fromDate, string toDate)
        {
            Warning[] warnings;
            string mimeType = null;
            string[] streamids;
            string encoding;
            string filenameExtension;

            var viewer = new ReportViewer();
            viewer.ProcessingMode = ProcessingMode.Local;
            viewer.SizeToReportContent = true;
            byte[] bytes = null;

            var from = Convert.ToDateTime(fromDate);
            var to = Convert.ToDateTime(toDate);

            using (var db = new ApplicationDbContext())
            {
                var wb = db.Waybills.Where(x => x.DateOfPickup >= from && x.DateOfPickup <= to).ToList();
                //viewer.LocalReport.ReportPath = @"../1ClickDelivery/Reports/InlineWaybills.rdlc";
                viewer.LocalReport.ReportPath = @"h:\root\home\epalabay-001\www\site1\Reports\InlineWaybills.rdlc";
                viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", wb));
                bytes = viewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                Response.Buffer = true;
                Response.Clear();
                Response.ContentType = mimeType;
                Response.AddHeader(
                    "content-disposition",
                    "attachment; filename= filename" + "." + filenameExtension);
                Response.OutputStream.Write(bytes, 0, bytes.Length); // create the file  
                Response.Flush(); // send it to the client to download  
                Response.End();
                return new FileContentResult(bytes, mimeType);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}