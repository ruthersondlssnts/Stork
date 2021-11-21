using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.Models
{
    [Table("VBMs")]
    public class VBM
    {
        [Key]
        public Guid PKVBM { get; set; }
        public Guid PKArea { get; set; }
        public string VBMName { get; set; }
    }
}