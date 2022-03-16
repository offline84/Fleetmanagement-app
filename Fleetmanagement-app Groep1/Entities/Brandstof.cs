using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Brandstof
    {
        [Key]
        public Guid Id {get; set;}

        [Required]
        [MaxLength(50)]
        public string TypeBrandstof {get; set;}

        public virtual ICollection<Voertuig> Voertuigen {get; set;}

        public virtual ICollection<ToewijzingBrandstofTankkaart> Toewijzingen {get; set;}


    }
}