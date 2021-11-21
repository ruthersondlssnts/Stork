using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _1ClickDelivery.Models;

namespace _1ClickDelivery.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RegisteredUser
        public ActionResult Index()
        {
            return View(db.RegisteredUsers.ToList());
        }

        public ActionResult Register([Bind(Include = "PKRegisteredUser,Email,FirstName,LastName,PhoneNo,Password")] RegisteredUser registeredUser)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    registeredUser.PKRegisteredUser = Guid.NewGuid();
                    registeredUser.DateCreated = DateTime.Now;
                    registeredUser.DateTimeCreated = DateTime.Now;
                    registeredUser.DateTimeUpdated = DateTime.Now;
                    registeredUser.LastLoginDateTime = DateTime.Now;
                    db.RegisteredUsers.Add(registeredUser);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(registeredUser);
            }
            catch (Exception ex) 
            {
                if (ex.InnerException.InnerException.Message.Contains("duplicate"))
                {
                    ViewBag.DuplicateEmail = "True";
                }
                return View("Register");
            }

            //return View(db.RegisteredUsers.ToList());
        }

        // GET: RegisteredUser/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            return View(registeredUser);
        }

        // GET: RegisteredUser/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RegisteredUser/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PKRegisteredUser,Email,FirstName,LastName,PhoneNo,Password,DateCreated,DateTimeCreated,DateTimeUpdated,LastLoginDateTime,IsActive")] RegisteredUser registeredUser)
        {
            if (ModelState.IsValid)
            {
                registeredUser.PKRegisteredUser = Guid.NewGuid();
                db.RegisteredUsers.Add(registeredUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(registeredUser);
        }

        // GET: RegisteredUser/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            return View(registeredUser);
        }

        // POST: RegisteredUser/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PKRegisteredUser,Email,FirstName,LastName,PhoneNo,Password,DateCreated,DateTimeCreated,DateTimeUpdated,LastLoginDateTime,IsActive")] RegisteredUser registeredUser)
        {
            if (ModelState.IsValid)
            {
                db.Entry(registeredUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(registeredUser);
        }

        // GET: RegisteredUser/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            if (registeredUser == null)
            {
                return HttpNotFound();
            }
            return View(registeredUser);
        }

        // POST: RegisteredUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RegisteredUser registeredUser = db.RegisteredUsers.Find(id);
            db.RegisteredUsers.Remove(registeredUser);
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
