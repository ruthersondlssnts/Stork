using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web; 

namespace _1ClickDelivery.Models
{
    [Table("Areas")]
    public class Area
    {
        [Key]
        public Guid PKArea { get; set; }
        public string AreaName { get; set; }
    }
}