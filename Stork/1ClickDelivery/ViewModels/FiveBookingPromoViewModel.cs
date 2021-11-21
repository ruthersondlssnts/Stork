using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.ViewModels
{
    public class FiveBookingPromoViewModel
    {
        [Key]
        public int Id { get; set; }
        public string SenderName { get; set; }
        public int BookingCount { get; set; }
    }
}