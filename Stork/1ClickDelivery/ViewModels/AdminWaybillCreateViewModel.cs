using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1ClickDelivery.ViewModels
{
    public class AdminWaybillCreateViewModel
    {
        //public Guid PKWayBill { get; set; }
        //public Guid PKSender { get; set; }
        //public int WayBillNo { get; set; }

        [Display(Name = "Manual Waybill#")]
        public string ManualWayBillNo { get; set; }

        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }

        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        public string SpecialInstruction { get; set; }

        [Required]
        [Display(Name = "Receiver Name")]
        [MaxLength(50, ErrorMessage = "Receiver Name cannot be longer than 50 characters.")]
        public string ReceiverName { get; set; }

        [Required]
        [Display(Name = "Receiver Phone#")]
        [MaxLength(15, ErrorMessage = "Phone# cannot be longer than 15 characters.")]
        public string ReceiverPhoneNo { get; set; }

        public string Status { get; set; }

        [Required]
        [Display(Name = "Pickup Date")]
        public DateTime DateOfPickup { get; set; }

        [Required]
        [Display(Name = "Pickup Area")]
        public string SelectedArea { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }

        [Required]
        [Display(Name = "Pickup Address")]
        public string PickupAddress { get; set; }

        [Required]
        [Display(Name = "Pickup Date")]
        public DateTime SelectedPickupDate { get; set; }
        public IEnumerable<SelectListItem> PickupDates { get; set; }

        [Display(Name = "Delivery Area")]
        public string SelectedDeliveryArea { get; set; }
        public IEnumerable<SelectListItem> DeliveryAreas { get; set; }

        [Required]
        [Display(Name = "Destination Address")]
        public string DestinationAddress { get; set; }

        //public DateTime? DateDelivered { get; set; }
    }
}