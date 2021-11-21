using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.Models
{
    [Table("ScheduledPickups")]
    public class ScheduledPickup
    {
        [Key]
        public Guid PKScheduledPickup { get; set; }
        public string SenderId { get; set; }

        [Display(Name = "Items")]
        public int NoItem { get; set; }


        [Display(Name = "Ref#")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReferenceNo { get; set; }

        public string SenderName { get; set; }

        [Display(Name = "Pickup Address")]
        public string PickupAddress { get; set; }

        [Display(Name = "Instruction")]
        public string SpecialInstruction { get; set; }

        public string PickupArea { get; set; }

        public string Status { get; set; }

        [Display(Name = "Pickup Date")]
        public DateTime DateOfPickup { get; set; }

        public string CreatedBy { get; set; }

        [Display(Name = "DateTime Created")]
        //[DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTime DateTimeCreated { get; set; }

        public DateTime? DateTimeUpdated { get; set; }
    }
}