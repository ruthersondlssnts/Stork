using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.Models
{
    [Table("Logs")]
    public class Log
    {
        [Key]
        public Guid PKLog { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public string Message { get; set; }
        public string CreatedBy { get; set; }
    }
}