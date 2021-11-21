using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _1ClickDelivery.ViewModels
{
    public class PickupAddressViewModel
    {
        [Key]
        public Guid PKPickupAddress { get; set; }
        public string Street { get; set; }
        public string Unit { get; set; }


        [Required]
        [Display(Name = "Area")]
        public string SelectedArea { get; set; }
        public IEnumerable<SelectListItem> Areas { get; set; }


        [Required]
        [Display(Name = "VillageBarangaMunicipality")]
        public string SelectedVillageBarangaMunicipality { get; set; }
        public IEnumerable<SelectListItem> VillageBarangaMunicipalitys { get; set; }

        [Required]
        public string ContactPerson { get; set; }

        [Required]
        public string ContactPersonNo { get; set; }


        public DateTime DateTimeCreated { get; set; }
    }
}