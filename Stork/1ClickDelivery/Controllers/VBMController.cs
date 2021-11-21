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
    public class VBMController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: VBM
        public ActionResult _IndexPartial(Guid id)
        {
            ViewBag.PKAreaID = id;
            List<VBM> barangays = db.VBMs.Where(x => x.PKArea == id).ToList();
            return PartialView(barangays);
        }

        // GET: VBM/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VBM vBM = db.VBMs.Find(id);
            if (vBM == null)
            {
                return HttpNotFound();
            }
            return View(vBM);
        }

        // GET: VBM/Create
        public ActionResult Create(Guid id)
        {
            
            ViewBag.PKAreaId = id;
            return View();
        }

        // POST: VBM/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PKVBM,PKArea,VBMName")] VBM vBM)
        {
            if (ModelState.IsValid)
            {
                vBM.PKVBM = Guid.NewGuid();
                db.VBMs.Add(vBM);
                db.SaveChanges();
                return RedirectToAction("Details", "Area", new { id = vBM.PKArea });

            }

            return View(vBM);
        }

        // GET: VBM/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VBM vBM = db.VBMs.Find(id);
            if (vBM == null)
            {
                return HttpNotFound();
            }
            return View(vBM);
        }

        // POST: VBM/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PKVBM,PKArea,VBMName")] VBM vBM)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vBM).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details","Area",new { id=vBM.PKArea});
            }
            return View(vBM);
        }

        // GET: VBM/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VBM vBM = db.VBMs.Find(id);
            if (vBM == null)
            {
                return HttpNotFound();
            }
            return View(vBM);
        }

        // POST: VBM/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VBM vBM = db.VBMs.Find(id);
            db.VBMs.Remove(vBM);
            db.SaveChanges();
            return RedirectToAction("Details", "Area", new { id = vBM.PKArea });
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
