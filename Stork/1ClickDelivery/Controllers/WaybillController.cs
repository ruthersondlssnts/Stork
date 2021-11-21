using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _1ClickDelivery.Models;
using _1ClickDelivery.UserClasses;
using Microsoft.AspNet.Identity;
using _1ClickDelivery.ViewModels;
using Microsoft.Reporting.WebForms;
using System.Text;

namespace _1ClickDelivery.Controllers
{
    public class WaybillController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Waybill
        public ActionResult Index()
        {
            return View(db.Waybills.ToList().OrderBy(x => x.WayBillNo));
        }


        public ActionResult _CreatePartial()
        {
            var getLists = new GetLists();
            ViewBag.Areas = getLists.GetAreas();
            var pas = getLists.GetPickupAddresses(User.Identity.GetUserId());
            var pickupDates = getLists.GetPickupDates();
            var w = new WaybillCreateViewModel() { PickupAddresses = pas, PickupDates = pickupDates };
            return PartialView(w);
        }

        public ActionResult _GridPartial()
        {
            var userId = User.Identity.GetUserId();
            var scheds = db.Waybills.Where(x => x.SenderId == userId);
            return PartialView(scheds.OrderByDescending(x => x.WayBillNo));
        }

        public FileResult PrintWaybill(string pkWaybill)
        {
            try
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

                var wb = db.Waybills.Where(x => x.PKWayBill.ToString() == pkWaybill).ToList();
                viewer.LocalReport.ReportPath = Server.MapPath("/Reports/Waybill.rdlc");
                //viewer.LocalReport.ReportPath = @"h:\root\home\epalabay-001\www\site1\Reports\Waybill.rdlc";
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
            catch (Exception ex)
            {
                db.Logs.Add(new Log { PKLog = Guid.NewGuid(), Message = ex.Message + " \n " + ex.InnerException, DateTimeCreated = DateTime.Now, CreatedBy = "Admin" });
                db.SaveChanges();
                throw;
            }

        }



        // GET: Waybill/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Waybill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WayBillNo, SelectedDeliveryArea, SelectedPickupAddress,SelectedPickupDate,SpecialInstruction,DestinationAddress,ReceiverName,ReceiverPhoneNo,Status")] WaybillCreateViewModel waybill)
        {
            var modelStateErrors = this.ModelState.Keys.SelectMany(key => this.ModelState[key].Errors);

            if (ModelState.IsValid)
            {
                var pkPA = new Guid(waybill.SelectedPickupAddress);
                var pa = db.PickupAddresses.Where(x => x.PKPickupAddress == pkPA).Single();
                var pickupAddress = pa.Unit + " " + pa.Street + " " + pa.VillageBarangaMunicipality + ", " + pa.Area + "- Contact:" + pa.ContactPerson + " " + pa.ContactPersonNo;

                var senderId = User.Identity.GetUserId();
                string senderName = string.Empty;
                //SenderName
                var email = User.Identity.Name;
                using (var i = new ApplicationDbContext())
                {
                    var u = i.Users.Where(x => x.Email == email).Single();
                    senderName = u.FirstName + " " + u.LastName;
                }

                var dArea = db.Areas.Where(x => x.PKArea.ToString() == waybill.SelectedDeliveryArea).First();
                var pk = Guid.NewGuid();
                var w = new Waybill()
                {
                    PKWayBill = pk,
                    PickupArea = pa.Area,
                    PickupAddress = pickupAddress,
                    DateOfPickup = waybill.SelectedPickupDate,
                    SpecialInstruction = waybill.SpecialInstruction,
                    ReceiverName = waybill.ReceiverName,
                    ReceiverPhoneNo = waybill.ReceiverPhoneNo,
                    DestinationAddress = waybill.DestinationAddress,
                    DeliveryArea = dArea.AreaName,
                    SenderId = senderId,
                    SenderName = senderName,
                    CreatedBy = email,
                    Status = "Scheduled",
                    DateTimeCreated = TimeZoneHelper.GetTodayWithTimeUTCPlus8()
                };
                db.Waybills.Add(w);
                db.SaveChanges();

                var sp1 = db.Waybills.Where(x => x.PKWayBill == pk).Single();
                var sb = new StringBuilder();
                sb.Append("A new waybill was created. "); sb.AppendLine();
                sb.Append("<br>SenderName: "); sb.Append(senderName); sb.AppendLine();
                sb.Append("<br>Date of Pickup:"); sb.Append(waybill.SelectedPickupDate.ToShortDateString());

                EmailHelper eh = new EmailHelper();
                eh.SendEmail(EmailHelper.TranType.Waybill, senderName, sp1.WayBillNo.ToString(), sb.ToString());

                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Dashboard");
        }


        // GET: Waybill/Delete/5
        public ActionResult Delete(Guid? id)
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

        // POST: Waybill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Waybill waybill = db.Waybills.Find(id);
            db.Waybills.Remove(waybill);
            db.SaveChanges();
            return RedirectToAction("Index");
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
