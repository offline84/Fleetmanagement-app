using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class ToewijzingBrandstofTankkaart
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Tankkaartnummer { get; set; }

        public virtual Tankkaart Tankkaart { get; set; }

        [Required]
        public Guid BrandstofId { get; set; }

        public virtual Brandstof Brandstof { get; set; }
    }
}