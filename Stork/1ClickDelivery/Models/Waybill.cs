using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.Models
{
    [Table("Waybills")]
    public class Waybill
    {
        string _specialInstruction = string.Empty;

        [Key]
        public Guid PKWayBill { get; set; }
        public string SenderId { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WayBillNo { get; set; }

        [Display(Name = "Manual Waybill#")]
        public string ManualWayBillNo { get; set; }
        [Required]
        [Display(Name = "Sender Name")]
        public string SenderName { get; set; }
        [Required]
        [Display(Name = "Pickup Area")]
        public string PickupArea { get; set; }
        [Required]
        [Display(Name = "Pickup Address")]
        public string PickupAddress { get; set; }

        [Display(Name = "Item Description")]
        public string ItemDescription { get; set; }

        [Display(Name = "Instruction")]
        public string SpecialInstruction { get { return _specialInstruction; } set { _specialInstruction = value == null ? string.Empty : value; } }
        [Required]
        [Display(Name = "Destination Address")]
        public string DestinationAddress { get; set; }

        [Required]
        [Display(Name = "Receiver Name")]
        public string ReceiverName { get; set; }

        [Required]
        [Display(Name = "Destination Area")]
        public string DeliveryArea { get; set; }

        [Required]
        [Display(Name = "Receiver Phone")]
        [MaxLength(15, ErrorMessage = "Name cannot be longer than 15 characters.")]
        public string ReceiverPhoneNo { get; set; }

        public string Status { get; set; }

        [Required]
        [Display(Name = "Pickup Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateOfPickup { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "DateTime Created")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeCreated { get; set; }

        public DateTime? DateTimeUpdated { get; set; }

        public DateTime? DateDelivered { get; set; }
    }
}