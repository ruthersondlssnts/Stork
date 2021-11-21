using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using _1ClickDelivery.Models;
using Microsoft.AspNet.Identity;
using _1ClickDelivery.ViewModels;

namespace _1ClickDelivery.Controllers
{
    [Authorize]
    public class PickupAddressController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: PickupAddresse
        public ActionResult Index()
        {
            return View(db.PickupAddresses.ToList());
        }

        private IEnumerable<SelectListItem> GetAreas()
        {
            //var a = db.Areas.ToList();

            List<SelectListItem> areas = db.Areas.AsNoTracking()
                .OrderBy(n => n.AreaName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.PKArea.ToString(),
                        Text = n.AreaName
                    }).ToList();
            var countrytip = new SelectListItem()
            {
                Value = "08A28167-AA0B-4D26-ADD4-D9E7A4EE3186",
                Text = "--- select area ---"
            };
            areas.Insert(0, countrytip);
            return new SelectList(areas, "Value", "Text");

        }

        private IEnumerable<SelectListItem> GetVBMs()
        {
            //var a = db.Areas.ToList();

            List<SelectListItem> vbms = db.VBMs.AsNoTracking()
                .OrderBy(n => n.VBMName)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.PKVBM.ToString(),
                        Text = n.VBMName
                    }).ToList();
            var countrytip = new SelectListItem()
            {
                Value = "08A28167-AA0B-4D26-ADD4-D9E7A4EE3186",
                Text = "--- select area ---"
            };
            vbms.Insert(0, countrytip);
            return new SelectList(vbms, "Value", "Text");

        }

        [HttpGet]
        public ActionResult GetRegions(string iso3)
        {
            //if (!string.IsNullOrWhiteSpace(iso3) && iso3.Length == 3)
            //{
            IEnumerable<SelectListItem> regions = GetRegions2(iso3);
            return Json(regions, JsonRequestBehavior.AllowGet);
            //}
            //return null;
        }

        private IEnumerable<SelectListItem> GetRegions2(string iso3)
        {
            if (!String.IsNullOrWhiteSpace(iso3))
            {
                using (var context = new ApplicationDbContext())
                {
                    IEnumerable<SelectListItem> regions = db.VBMs.AsNoTracking()
                        .OrderBy(n => n.VBMName)
                        .Where(n => n.PKArea.ToString() == iso3)
                        .Select(n =>
                           new SelectListItem
                           {
                               Value = n.PKVBM.ToString(),
                               Text = n.VBMName
                           }).ToList();


                    return new SelectList(regions, "Value", "Text");
                }
            }
            return null;
        }

        [OutputCache(Duration = 1)]
        public ActionResult _IndexPartial()
        {
            var userId = User.Identity.GetUserId();
            var addresses = db.PickupAddresses.Where(x => x.SenderId == userId).ToList();
            return PartialView(addresses);
        }

        public ActionResult _CreatePartial()
        {
            var areas = GetAreas();
            var vbms = GetVBMs();
            var address = new PickupAddressViewModel() { Areas = areas, VillageBarangaMunicipalitys = vbms };
            return PartialView(address);
        }

        [OutputCache(Duration = 1)]
        public ActionResult _EditPartial(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pa = db.PickupAddresses.Find(id);
            var areas = GetAreas();
            var pkSelectedArea = areas.Where(x => x.Text == pa.Area).SingleOrDefault().Value;
            var vbms = GetVBMs();
            var pkVbm = vbms.Where(x => x.Text == pa.VillageBarangaMunicipality).SingleOrDefault().Value;

            var pavm = new PickupAddressViewModel() { PKPickupAddress = pa.PKPickupAddress, Street = pa.Street, ContactPerson = pa.ContactPerson, ContactPersonNo = pa.ContactPersonNo, Areas = areas, VillageBarangaMunicipalitys = vbms, SelectedArea = pkSelectedArea, SelectedVillageBarangaMunicipality = pkVbm, Unit = pa.Unit };

            var address = new PickupAddressViewModel() { Areas = areas, VillageBarangaMunicipalitys = vbms };
            if (pa == null)
            {
                return HttpNotFound();
            }
            return PartialView(pavm);
        }



        // GET: PickupAddresse/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PickupAddresse/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "PKPickupAddress,Street,Unit,SelectedArea,SelectedVillageBarangaMunicipality,ContactPerson,ContactPersonNo,DateTimeCreated")] PickupAddress pickupAddress)
        public ActionResult Create([Bind(Include = "PKPickupAddress,Street,Unit,SelectedArea,SelectedVillageBarangaMunicipality,ContactPerson,ContactPersonNo,DateTimeCreated")] PickupAddressViewModel pickupAddress)
        {
            if (ModelState.IsValid)
            {
                var senderId = User.Identity.GetUserId();
                var areaName = db.Areas.Where(x => x.PKArea.ToString() == pickupAddress.SelectedArea).SingleOrDefault().AreaName;
                var vbmName = db.VBMs.Where(x => x.PKVBM.ToString() == pickupAddress.SelectedVillageBarangaMunicipality).SingleOrDefault().VBMName;

                var pa = new PickupAddress
                {
                    PKPickupAddress = Guid.NewGuid(),
                    SenderId = senderId,
                    Area = areaName,
                    VillageBarangaMunicipality = vbmName,
                    Street = pickupAddress.Street,
                    Unit = pickupAddress.Unit,
                    ContactPerson = pickupAddress.ContactPerson,
                    ContactPersonNo = pickupAddress.ContactPersonNo,
                    DateTimeCreated = DateTime.Today
                };

                db.PickupAddresses.Add(pa);
                db.SaveChanges();

                TempData["PartialToLoad"] = "_PickupAddressIndexPartial";
                return RedirectToAction("Index", "Dashboard");


            }

            return View(pickupAddress);
        }



        // POST: PickupAddresse/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PKPickupAddress,Street,Unit,SelectedArea,SelectedVillageBarangaMunicipality,ContactPerson,ContactPersonNo,DateTimeCreated")] PickupAddressViewModel pickupAddress)
        {
            if (ModelState.IsValid)
            {
                var areaName = db.Areas.Where(x => x.PKArea.ToString() == pickupAddress.SelectedArea).SingleOrDefault().AreaName;
                var vbmName = db.VBMs.Where(x => x.PKVBM.ToString() == pickupAddress.SelectedVillageBarangaMunicipality).SingleOrDefault().VBMName;
                var p = db.PickupAddresses.Where(x => x.PKPickupAddress == pickupAddress.PKPickupAddress).SingleOrDefault();
                p.Street = pickupAddress.Street;
                p.Unit = pickupAddress.Unit;
                p.Area = areaName;
                p.VillageBarangaMunicipality = vbmName;
                p.ContactPerson = pickupAddress.ContactPerson;
                p.ContactPersonNo = pickupAddress.ContactPersonNo;

                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                TempData["PartialToLoad"] = "_PickupAddressIndexPartial";
                return RedirectToAction("Index", "Dashboard");
            }
            //return View(pickupAddress);
            TempData["PartialToLoad"] = "_PickupAddressIndexPartial";
            return RedirectToAction("Index", "Dashboard");
        }

        // GET: PickupAddresse/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PickupAddress pickupAddress = db.PickupAddresses.Find(id);
            if (pickupAddress == null)
            {
                return HttpNotFound();
            }
            return View(pickupAddress);
        }

        // POST: PickupAddresse/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            PickupAddress pickupAddress = db.PickupAddresses.Find(id);
            db.PickupAddresses.Remove(pickupAddress);
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
