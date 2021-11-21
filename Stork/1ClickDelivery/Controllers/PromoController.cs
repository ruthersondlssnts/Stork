using _1ClickDelivery.Models;
using _1ClickDelivery.UserClasses;
using _1ClickDelivery.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1ClickDelivery.Controllers
{
    public class PromoController : Controller
    {
        DateTime _from = TimeZoneHelper.GetTodayUTCPlus8();
        DateTime _to = TimeZoneHelper.GetTodayUTCPlus8();

        // GET: Promo
        public ActionResult Index()
        {
            return View();
        }

        private void SetParam(out DateTime from, string fromDate, out DateTime to, string toDate)
        {
            from = TimeZoneHelper.GetTodayUTCPlus8();
            to = TimeZoneHelper.GetTodayUTCPlus8();
            if (fromDate != null) from = Convert.ToDateTime(fromDate);
            if (toDate != null) to = Convert.ToDateTime(toDate);
        }

        public ActionResult FiveBookingPromo(string fromDate, string toDate)
        {
            SetParam(out _from, fromDate, out _to, toDate);
            using (var db = new ApplicationDbContext())
            {
                //Scheduled Pickups
                //var sps = db.ScheduledPickups.Where(x => (x.DateOfPickup >= _from && x.DateOfPickup <= _to)).ToList();
                var sps = db.ScheduledPickups.ToList();

                //Waybills
                //var wbs = (from o in db.Waybills
                //           where (o.DateOfPickup >= _from && o.DateOfPickup <= _to)
                //           select new { o.DateOfPickup, o.PickupAddress, o.SenderName, o.SpecialInstruction, o.Status, o.DateTimeCreated }).ToList();
                var wbs = (from o in db.Waybills
                           select new { o.DateOfPickup, o.PickupAddress, o.SenderName, o.SpecialInstruction, o.Status, o.DateTimeCreated }).ToList();


                if (wbs != null)
                {
                    foreach (var item in wbs)
                    {
                        sps.Add(new ScheduledPickup
                        {
                            DateOfPickup = item.DateOfPickup,
                            PickupAddress = item.PickupAddress,
                            SenderName = item.SenderName,
                            SpecialInstruction = item.SpecialInstruction,
                            Status = item.Status,
                            DateTimeCreated = item.DateTimeCreated
                        });
                    }
                }

                //var d = sps.GroupBy(x => x.SenderName).Select(group => group.Count(item=>item.PKScheduledPickup == item.PKScheduledPickup)).ToList();
                var d = sps.GroupBy(x => x.SenderName).Select(g => new {g.Key, Count = g.Count() }).ToList();
                var fbps = new List<FiveBookingPromoViewModel>();
                foreach (var item in d)
                {
                    var e = new FiveBookingPromoViewModel() {SenderName=item.Key, BookingCount = item.Count };
                    fbps.Add(e);
                }

                return View(fbps);
            }
        }

        // GET: Promo/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Promo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Promo/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Promo/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Promo/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Promo/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Promo/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
