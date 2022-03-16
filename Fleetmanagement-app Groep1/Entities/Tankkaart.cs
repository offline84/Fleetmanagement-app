using Microsoft.EntityFrameworkCore;
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
        [MaxLength(8), MinLength(4)]
        public int Pincode {get; set;}

        public virtual ICollection<ToewijzingBrandstofTankkaart> MogelijkeBrandstoffen {get; set;}
            = new List<ToewijzingBrandstofTankkaart>();

        public virtual Koppeling Koppeling {get; set;}

        public bool IsGeblokkeerd {get; set;}

        public bool IsGearchiveerd {get; set;}

        [Timestamp]
        public DateTime LaatstGeupdate {get; set;}
    }

   
    public class ToewijzingBrandstofTankkaart
    {
        [Key]
        public Guid Id {get; set;}

        [Required]
        public string TankkaartId {get; set;}

        public virtual Tankkaart Tankkaart {get; set;}

        [Required]
        public Guid BrandstofId { get; set; }

        public virtual Brandstof Brandstof {get; set;}
    }
}