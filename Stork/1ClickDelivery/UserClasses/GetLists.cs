using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using _1ClickDelivery.Models;

namespace _1ClickDelivery.UserClasses
{
    public class GetLists
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        public IEnumerable<SelectListItem> GetPickupAddresses(string senderId)
        {
            var vbms = db.PickupAddresses.AsNoTracking()
                .Where(x => x.SenderId == senderId)
                .OrderBy(n => n.Unit)
                    .Select(n =>
                    new SelectListItem
                    {
                        //Value = n.Unit + " " + n.Street + " " + n.VillageBarangaMunicipality + "--" + n.ContactPerson + " " + n.ContactPersonNo,
                        //Value = n.PKPickupAddress.ToString() + n.Unit + " " + n.Street + " " + n.VillageBarangaMunicipality + ", " + n.Area + "- Contact:" + n.ContactPerson + " " + n.ContactPersonNo,
                        Value = n.PKPickupAddress.ToString(),
                        Text = n.Unit + " " + n.Street + " " + n.VillageBarangaMunicipality + ", " + n.Area + "- Contact:" + n.ContactPerson + " " + n.ContactPersonNo
                    }).ToList();
            var countrytip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Area ---"
            };
            vbms.Insert(0, countrytip);
            return new SelectList(vbms, "Value", "Text");
        }

        public IEnumerable<SelectListItem> GetPickupDates()
        {
            var start = TimeZoneHelper.GetTodayUTCPlus8();
            var cutoff = new TimeSpan(14, 0, 0); //2PM
            if (TimeZoneHelper.GetTodayWithTimeUTCPlus8().TimeOfDay > cutoff)
                start = start.AddDays(1);


            var end = start.AddDays(5);
            var startstring = start;
            var vbms = db.PickupDates.AsNoTracking()
                .Where(x => x.Date >= start && x.Date <= end)
                .OrderBy(n => n.Date).ToList()
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Date.ToShortDateString(),
                        Text = n.Date.ToString("MMMM dd yyyy")
                    }).ToList();


            var countrytip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Pickup Date ---"
            };
            vbms.Insert(0, countrytip);
            return new SelectList(vbms, "Value", "Text");

        }

        public IEnumerable<SelectListItem> GetPickupDatesForAdmin()
        {
            var start = TimeZoneHelper.GetTodayUTCPlus8().AddDays(-10);
            //var cutoff = new TimeSpan(14, 0, 0); //2PM
            //if (TimeZoneHelper.GetTodayWithTimeUTCPlus8().TimeOfDay > cutoff)
            //    start = start.AddDays(1);

            var end = start.AddDays(15);
            var startstring = start.ToShortDateString();
            var vbms = db.PickupDates.AsNoTracking()
                .Where(x => x.Date >= start && x.Date <= end)
                .OrderBy(n => n.Date)
                    .Select(n =>
                    new SelectListItem
                    {
                        Value = n.Date.ToString(),
                        Text = n.Date.ToString()
                    }).ToList();
            var countrytip = new SelectListItem()
            {
                Value = null,
                Text = "--- Select Pickup Date ---"
            };
            vbms.Insert(0, countrytip);
            return new SelectList(vbms, "Value", "Text");

        }

        public IEnumerable<SelectListItem> GetAreas()
        {
            using (var db = new ApplicationDbContext())
            {
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
        }
    }
}