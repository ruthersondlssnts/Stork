using _1ClickDelivery.Models;
using _1ClickDelivery.UserClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoreLinq;
using _1ClickDelivery.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Reporting.WebForms;

namespace _1ClickDelivery.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : Controller
    {
        DateTime _from = TimeZoneHelper.GetTodayUTCPlus8();
        DateTime _to = TimeZoneHelper.GetTodayUTCPlus8();
        string _stat = "Scheduled";

        // GET: AdminDashboard
        public ActionResult Index()
        {
            ViewBag.FromDate = TimeZoneHelper.GetTodayUTCPlus8String();
            ViewBag.ToDate = TimeZoneHelper.GetTodayUTCPlus8String();
            return View();
        }

        private void SetParam(out DateTime from, string fromDate, out DateTime to, string toDate, out string stat, string status)
        {
            from = TimeZoneHelper.GetTodayUTCPlus8();
            to = TimeZoneHelper.GetTodayUTCPlus8();
            stat = "Scheduled";
            if (fromDate == "" || fromDate == null)
            {
                ViewBag.FromDate = TimeZoneHelper.GetTodayUTCPlus8String();
                from = TimeZoneHelper.GetTodayUTCPlus8();
            }
            else
                from = Convert.ToDateTime(fromDate);

            if (toDate == "" || toDate == null)
            {
                ViewBag.ToDate = TimeZoneHelper.GetTodayUTCPlus8String();
                to = TimeZoneHelper.GetTodayUTCPlus8();
            }
            else
                to = Convert.ToDateTime(toDate);

            if (status != "") stat = status;
        }

        private void SetParam(out DateTime from, string fromDate, out DateTime to, string toDate)
        {
            from = TimeZoneHelper.GetTodayUTCPlus8();
            to = TimeZoneHelper.GetTodayUTCPlus8();
            if (fromDate == "" || fromDate == null)
            {
                ViewBag.FromDate = TimeZoneHelper.GetTodayUTCPlus8String();
                from = TimeZoneHelper.GetTodayUTCPlus8();
            }
            else
                from = Convert.ToDateTime(fromDate);

            if (toDate == "" || toDate == null)
            {
                ViewBag.ToDate = TimeZoneHelper.GetTodayUTCPlus8String();
                to = TimeZoneHelper.GetTodayUTCPlus8();
            }
            else
                to = Convert.ToDateTime(toDate);
        }

        public ActionResult _ScheduledPickupsSummaryPartial(string fromDate, string toDate)
        {
            SetParam(out _from, fromDate, out _to, toDate);
            var sps = GetScheduledPickupSummary(_from, _to);
            return PartialView(sps);
            //using (var db = new ApplicationDbContext())
            //{
            //    //Scheduled Pickups
            //    var sps = db.ScheduledPickups.Where(x => (x.DateOfPickup >= _from && x.DateOfPickup <= _to) && x.Status == "Scheduled").ToList();

            //    //Waybills
            //    var wbs = (from o in db.Waybills
            //               where (o.DateOfPickup >= _from && o.DateOfPickup <= _to) && o.Status == "Scheduled"
            //               select new { o.PickupArea, o.WayBillNo, o.DateOfPickup, o.PickupAddress, o.SenderName, o.SpecialInstruction, o.Status, o.DateTimeCreated }).ToList();
            //    if (wbs != null)
            //    {
            //        foreach (var item in wbs)
            //        {
            //            sps.Add(new ScheduledPickup
            //            {
            //                PickupArea = item.PickupArea,
            //                ReferenceNo = item.WayBillNo,
            //                DateOfPickup = item.DateOfPickup,
            //                PickupAddress = item.PickupAddress,
            //                SenderName = item.SenderName,
            //                SpecialInstruction = item.SpecialInstruction,
            //                Status = item.Status,
            //                DateTimeCreated = item.DateTimeCreated
            //            });
            //        }
            //    }

            //    var d = sps.DistinctBy(x => x.DateOfPickup + "." + x.PickupAddress);
            //    return PartialView(d.OrderBy(x => x.DateOfPickup));
            //}
        }

        private List<ScheduledPickup> GetScheduledPickupSummary(DateTime fromDate, DateTime toDate)
        {
            //SetParam(out _from, fromDate, out _to, toDate);
            using (var db = new ApplicationDbContext())
            {
                //Scheduled Pickups
                var sps = db.ScheduledPickups.Where(x => (x.DateOfPickup >= fromDate && x.DateOfPickup <= toDate) && x.Status == "Scheduled").ToList();

                //Waybills
                var wbs = (from o in db.Waybills
                           where (o.DateOfPickup >= fromDate && o.DateOfPickup <= toDate) && o.Status == "Scheduled"
                           select new { o.PickupArea, o.WayBillNo, o.DateOfPickup, o.PickupAddress, o.SenderName, o.SpecialInstruction, o.Status, o.DateTimeCreated }).ToList();
                if (wbs != null)
                {
                    foreach (var item in wbs)
                    {
                        sps.Add(new ScheduledPickup
                        {
                            PickupArea = item.PickupArea,
                            ReferenceNo = item.WayBillNo,
                            DateOfPickup = item.DateOfPickup,
                            PickupAddress = item.PickupAddress,
                            SenderName = item.SenderName,
                            SpecialInstruction = item.SpecialInstruction,
                            Status = item.Status,
                            DateTimeCreated = item.DateTimeCreated
                        });
                    }
                }

                var scheduledPickups = sps.DistinctBy(x => x.DateOfPickup + "." + x.PickupAddress).ToList();
                return scheduledPickups;
            }
        }

        private List<ScheduledPickup> GetScheduledPickups(DateTime fromDate, DateTime toDate, string stat)
        {
            //SetParam(out _from, fromDate, out _to, toDate);
            using (var db = new ApplicationDbContext())
            {
                //Scheduled Pickups
                var sps = db.ScheduledPickups.Where(x => (x.DateOfPickup >= fromDate && x.DateOfPickup <= toDate) && x.Status == stat).ToList();
                //var scheduledPickups = sps.DistinctBy(x => x.DateOfPickup + "." + x.PickupAddress).ToList();
                return sps;
            }
        }

        public ActionResult _ScheduledPickupsPartial(string fromDate, string toDate, string status)
        {
            SetParam(out _from, fromDate, out _to, toDate, out _stat, status);
            using (var db = new ApplicationDbContext())
            {
                var sp = db.ScheduledPickups.Where(x => x.Status.Trim() == _stat.Trim()
                && (x.DateOfPickup >= _from && x.DateOfPickup <= _to)).OrderByDescending(x => x.ReferenceNo).ToList();
                return PartialView(sp);
            }
        }



        public ActionResult _WaybillsPartial(string fromDate, string toDate, string status)
        {
            SetParam(out _from, fromDate, out _to, toDate, out _stat, status);
            using (var db = new ApplicationDbContext())
            {
                var sp = db.Waybills.Where(x => x.Status == _stat
                && (x.DateOfPickup >= _from && x.DateOfPickup <= _to)).OrderByDescending(x => x.WayBillNo).ToList();
                return PartialView(sp);
            }
        }



        [HttpPost]
        public ActionResult _ScheduledPickupsRefreshPartial(string fromDate, string toDate, string status)
        {
            SetParam(out _from, fromDate, out _to, toDate, out _stat, status);
            using (var db = new ApplicationDbContext())
            {
                var sp = db.ScheduledPickups.Where(x => x.Status == _stat
                && (x.DateOfPickup >= _from && x.DateOfPickup <= _to)).OrderByDescending(x => x.DateTimeCreated).ToList();
                return PartialView("_ScheduledPickupsPartial", sp);
            }
        }

        [HttpPost]
        public ActionResult _WaybillsRefreshPartial(string fromDate, string toDate, string status)
        {
            SetParam(out _from, fromDate, out _to, toDate, out _stat, status);
            using (var db = new ApplicationDbContext())
            {
                var sp = db.Waybills.Where(x => x.Status == _stat
                && (x.DateOfPickup >= _from && x.DateOfPickup <= _to)).OrderByDescending(x => x.WayBillNo).ToList();
                return PartialView("_WaybillsPartial", sp);
            }
        }




        [HttpPost]
        public ActionResult UpdateStatusSP(string fromDate, string toDate, string status, string selStat, Guid pk)
        {
            SetParam(out _from, fromDate, out _to, toDate, out _stat, status);

            using (var db = new ApplicationDbContext())
            {
                var sp1 = db.ScheduledPickups.Where(x => x.PKScheduledPickup == pk).FirstOrDefault();
                sp1.Status = selStat;
                db.SaveChanges();

                var sp = db.ScheduledPickups.Where(x => x.Status == _stat
                && (x.DateOfPickup >= _from && x.DateOfPickup <= _to)).ToList();
                return PartialView("_ScheduledPickupsPartial", sp);
            }
        }

        [HttpPost]
        public ActionResult UpdateStatusWB(string fromDate, string toDate, string status, string selStat, Guid pk)
        {
            SetParam(out _from, fromDate, out _to, toDate, out _stat, status);

            using (var db = new ApplicationDbContext())
            {
                var sp1 = db.Waybills.Where(x => x.PKWayBill == pk).FirstOrDefault();
                sp1.Status = selStat;
                db.SaveChanges();

                var sp = db.Waybills.Where(x => x.Status == status
                && (x.DateOfPickup >= _from && x.DateOfPickup <= _to)).ToList();
                return PartialView("_WaybillsPartial", sp);
            }
        }

        [HttpPost]
        public FileResult PrintScheduledPickupsSummary(string fromDate, string toDate)
        {
            //var printHelper = new Printing();
            //return PrintScheduledPickupSummary(fromDate, toDate);

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
                //var wb = db.ScheduledPickups.Where(x => x.DateOfPickup >= from && x.DateOfPickup <= to).ToList();
                var sps = GetScheduledPickupSummary(from, to);

                //viewer.LocalReport.ReportPath = @"../1ClickDelivery/Reports/ScheduledPickupsSummary.rdlc";
                viewer.LocalReport.ReportPath = @"h:\root\home\epalabay-001\www\site1\Reports\ScheduledPickupsSummary.rdlc";

                //ReportParameter p1 = new ReportParameter("FromDate", from.ToShortDateString());
                //ReportParameter p2 = new ReportParameter("ToDate", to.ToShortDateString());
                //viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });


                //ReportParameter[] parameters = new ReportParameter[1];
                //parameters[0] = new ReportParameter("FromDate", from.ToShortDateString());


                viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", sps));
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

        [HttpPost]
        public FileResult PrintScheduledPickups(string fromDate, string toDate, string status)
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
                //var wb = db.ScheduledPickups.Where(x => x.DateOfPickup >= from && x.DateOfPickup <= to).ToList();
                var sps = GetScheduledPickups(from, to, status);
                //viewer.LocalReport.ReportPath = @"../1ClickDelivery/Reports/ScheduledPickups.rdlc";
                viewer.LocalReport.ReportPath = @"h:\root\home\epalabay-001\www\site1\Reports\ScheduledPickups.rdlc";

                //ReportParameter p1 = new ReportParameter("FromDate", from.ToShortDateString());
                //ReportParameter p2 = new ReportParameter("ToDate", to.ToShortDateString());
                //viewer.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });

                viewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", sps));
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


    }
}
