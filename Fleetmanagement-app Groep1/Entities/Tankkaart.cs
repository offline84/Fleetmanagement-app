using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Tankkaart
    {
        [Key]
        public string Kaartnummer {get; set;}

        [Required]
        [Timestamp]
        public DateTime GeldigheidsDatum {get; set;}

        [Required]
        [MaxLength(6)]
        [MinLength(4)]
        public int Pincode {get; set;}

        public ICollection<Brandstof> MogelijkeBrandstoffen {get; set;}
            = new List<Brandstof>();

        public string? BestuurdersId {get; set;}

        public Bestuurder Bestuurder {get; set;}

        public bool IsGeblokkeerd {get; set;}

        public bool IsGearchiveerd {get; set;}

        [Timestamp]
        public DateTime LaatstGeupdate {get; set;}
    }
}