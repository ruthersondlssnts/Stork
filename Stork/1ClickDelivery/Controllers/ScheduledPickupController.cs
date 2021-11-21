using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _1ClickDelivery.Models;
using _1ClickDelivery.ViewModels;
using Microsoft.AspNet.Identity;
using _1ClickDelivery.UserClasses;
using System.Text;

namespace _1ClickDelivery.Controllers
{
    [Authorize]
    public class ScheduledPickupController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ScheduledPickup
        public ActionResult Index()
        {

            return View(db.ScheduledPickups.ToList().OrderBy(x => x.DateOfPickup));
        }

        [OutputCache(Duration = 1)]
        public ActionResult _CreatePartial()
        {
            var pas = new GetLists().GetPickupAddresses(User.Identity.GetUserId());
            var pickupDates = new GetLists().GetPickupDates();
            var addresses = new ScheduleAPickupCreateViewModel() { PickupAddresses = pas, PickupDates = pickupDates };
            return PartialView(addresses);
        }

        public ActionResult _GridPartial()
        {
            var userId = User.Identity.GetUserId();
            var scheds = db.ScheduledPickups.Where(x => x.SenderId == userId);
            return PartialView(scheds.OrderBy(x => x.DateOfPickup));
        }



        // GET: ScheduledPickup/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ScheduledPickup/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SelectedPickupAddress, PickupAddress,NoItem,SpecialInstruction,SelectedPickupDate")] ScheduleAPickupCreateViewModel pickup)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //Pickup Address
                    var pkPA = new Guid(pickup.SelectedPickupAddress);
                    var pa = db.PickupAddresses.Where(x => x.PKPickupAddress == pkPA).Single();
                    var pickupAdd = pa.Unit + " " + pa.Street + " " + pa.VillageBarangaMunicipality + ", " + pa.Area + "- Contact:" + pa.ContactPerson + " " + pa.ContactPersonNo;

                    var senderId = User.Identity.GetUserId();
                    string senderName = string.Empty;
                    //SenderName
                    var email = User.Identity.Name;
                    using (var i = new ApplicationDbContext())
                    {
                        var u = i.Users.Where(x => x.Email == email).Single();
                        senderName = u.FirstName + " " + u.LastName;
                    }

                    var pk = Guid.NewGuid();
                    var sp = new ScheduledPickup()
                    {
                        PKScheduledPickup = pk,
                        SenderId = senderId,
                        SenderName = senderName,
                        CreatedBy = email,
                        PickupArea = pa.Area,
                        PickupAddress = pickupAdd,
                        NoItem = pickup.NoItem,
                        SpecialInstruction = pickup.SpecialInstruction,
                        DateOfPickup = pickup.SelectedPickupDate,
                        DateTimeCreated = TimeZoneHelper.GetTodayWithTimeUTCPlus8(),
                        Status = "Scheduled"
                    };
                    db.ScheduledPickups.Add(sp);
                    db.SaveChanges();

                    var sp1 = db.ScheduledPickups.Where(x => x.PKScheduledPickup == pk).Single();
                    var sb = new StringBuilder();
                    sb.Append("A new pickup schedule was created. "); sb.AppendLine();
                    sb.Append("<br>SenderName: "); sb.Append(senderName); sb.AppendLine();
                    sb.Append("<br>Date of Pickup:"); sb.Append(pickup.SelectedPickupDate.ToShortDateString());

                    var eh = new EmailHelper();
                    eh.SendEmail(EmailHelper.TranType.Schedule, senderName, sp1.ReferenceNo.ToString(), sb.ToString());

                    return RedirectToAction("Index", "Dashboard");
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex);
                //Redirect to error page
                return RedirectToAction("Index", "Dashboard");
            }

            return RedirectToAction("Index", "Dashboard");


        }

        // GET: ScheduledPickup/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ScheduledPickup scheduledPickup = db.ScheduledPickups.Find(id);
            if (scheduledPickup == null)
            {
                return HttpNotFound();
            }
            return View(scheduledPickup);
        }

        // POST: ScheduledPickup/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PKScheduledPickup,NoItem,SenderName,PickupAddress,SpecialInstruction,Status,DateOfPickup,DateTimeCreated,DateTimeUpdated")] ScheduledPickup scheduledPickup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(scheduledPickup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(scheduledPickup);
        }

        // GET: ScheduledPickup/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var scheduledPickup = db.ScheduledPickups.Find(id);
            if (scheduledPickup == null)
            {
                return HttpNotFound();
            }
            return View(scheduledPickup);
        }

        // POST: ScheduledPickup/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            var scheduledPickup = db.ScheduledPickups.Find(id);
            db.ScheduledPickups.Remove(scheduledPickup);
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
