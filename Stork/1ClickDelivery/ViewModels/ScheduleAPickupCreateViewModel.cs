using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1ClickDelivery.ViewModels
{
    public class ScheduleAPickupCreateViewModel
    {
        public Guid PKScheduledPickup { get; set; }
        public string SenderId { get; set; }


        [Range(1, 500)]
        [Display(Name = "Number of Items")]
        public int NoItem { get; set; }

        public string SenderName { get; set; }

        [Display(Name = "Pickup Address")]
        public string PickupAddress { get; set; }

        [Display(Name = "Special Instruction")]
        public string SpecialInstruction { get; set; }

        public string Status { get; set; }

        [Required]
        [Display(Name = "Pickup Date")]
        public DateTime SelectedPickupDate { get; set; }
        public IEnumerable<SelectListItem> PickupDates { get; set; }

        [Required]
        [Display(Name = "Pickup Address")]
        public string SelectedPickupAddress { get; set; }
        public IEnumerable<SelectListItem> PickupAddresses { get; set; }
    }
}