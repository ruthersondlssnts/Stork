using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace _1ClickDelivery.Models
{
    [Table("RegisteredUsers")]
    public class RegisteredUser
    {
        [Key]
        public Guid PKRegisteredUser { get; set; }

        [Required]
        public string Email { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PhoneNo { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeUpdated { get; set; }
        public DateTime LastLoginDateTime { get; set; }
        public bool IsActive { get; set; }

    }
}