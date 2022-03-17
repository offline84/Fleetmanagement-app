using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Brandstof
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id {get; set;}

        [Required]
        [MaxLength(50)]
        public string TypeBrandstof {get; set;}

        public virtual ICollection<Voertuig> Voertuigen {get; set;}

        public virtual ICollection<ToewijzingBrandstofTankkaart> Toewijzingen {get; set;}


    }
}