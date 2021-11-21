using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.Models
{
    [Table("PickupDates")]
    public class PickupDate
    {
        [Key]
        public Guid PKPickupDate { get; set; }

       [Column("Date")]
        public DateTime Date { get; set; }
    }
}