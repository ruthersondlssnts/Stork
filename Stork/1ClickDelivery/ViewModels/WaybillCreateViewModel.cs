using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1ClickDelivery.ViewModels
{
    public class WaybillCreateViewModel
    {

        public Guid PKWayBill { get; set; }
        public Guid PKSender { get; set; }
        public int WayBillNo { get; set; }
        public string SenderName { get; set; }
        public string PickupAddress { get; set; }
        public string SpecialInstruction { get; set; }

        [Required]
        public string DestinationAddress { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "Receiver Name cannot be longer than 50 characters.")]
        public string ReceiverName { get; set; }      

        //[Required]
        //[MaxLength(20, ErrorMessage = "Delivery Area cannot be longer than 20 characters.")]
        //public string DeliveryArea { get; set; }

        [Required]
        [MaxLength(15, ErrorMessage = "Phone# cannot be longer than 15 characters.")]
        public string ReceiverPhoneNo { get; set; }

        public string Status { get; set; }

        [Required]
        public DateTime DateOfPickup { get; set; }

        [Required]
        [Display(Name = "PickupAddress")]
        public string SelectedPickupAddress { get; set; }
        public IEnumerable<SelectListItem> PickupAddresses { get; set; }

        //[Required]
        [Display(Name = "Delivery Area")]
        public string SelectedDeliveryArea { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }

        [Required]
        [Display(Name = "Pickup Date")]
        public DateTime SelectedPickupDate { get; set; }
        public IEnumerable<SelectListItem> PickupDates { get; set; }

        public DateTime? DateDelivered { get; set; }

    }
}