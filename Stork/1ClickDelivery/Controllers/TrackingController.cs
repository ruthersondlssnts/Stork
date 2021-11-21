using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _1ClickDelivery.Models;

namespace _1ClickDelivery.Controllers
{
    public class TrackingController : Controller
    {
        // GET: Tracking
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult _DeliveredPartial()
        {
            return PartialView();
        }

        public ActionResult _CollectedPartial()
        {
            return PartialView();
        }

        public ActionResult _CancelledPartial()
        {
            return PartialView();
        }

        public ActionResult _WaybillNotFound()
        {
            return PartialView();
        }
        public ActionResult _EnroutePartial()
        {
            return PartialView("_EnroutePartial");
        }

        [HttpPost]
        public ActionResult SearchWaybill(int wayBillNo)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Waybill d = new Waybill();

                    if (wayBillNo.ToString().Length == 7)
                        d = db.Waybills.Where(x => x.ManualWayBillNo == wayBillNo.ToString()).DefaultIfEmpty().First();
                    else
                        d = db.Waybills.Where(x => x.WayBillNo == wayBillNo).DefaultIfEmpty().First();

                    if (d == null)
                    {
                        return PartialView("_WaybillNotFound");
                    }

                    if (d.Status == "Collection")
                    {
                        return PartialView("_CollectionPartial");
                    }

                    if (d.Status == "Delivery")
                    {
                        return PartialView("_DeliveryPartial");
                    }
                    else if (d.Status == "Delivered")
                    {
                        return PartialView("_DeliveredPartial");
                    }
                    else if (d.Status.ToUpper() == "COLLECTED")
                    {
                        return PartialView("_CollectedPartial");
                    }
                    else if (d.Status.ToUpper() == "CANCELLED")
                    {
                        return PartialView("_CancelledPartial");
                    }
                    else
                    {
                        return PartialView("_WaybillNotFound");
                    }
                }
            }
            catch (Exception)
            {
                //Do something here
                return PartialView("_WaybillNotFound");
            }
        }

    }
}