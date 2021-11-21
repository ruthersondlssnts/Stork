using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.Models
{
    [Table("PickupAddresses")]
    public class PickupAddress
    {
        [Key]
        public Guid PKPickupAddress { get; set; }
        public string SenderId { get; set; }

        public string Street { get; set; }

        public string Unit { get; set; }

        [Required]
        public string Area { get; set; }

        [Required]
        public string VillageBarangaMunicipality { get; set; }

        [Required]
        public string ContactPerson { get; set; }

        [Required]
        public string ContactPersonNo { get; set; }
        public DateTime DateTimeCreated { get; set; }

    }
}