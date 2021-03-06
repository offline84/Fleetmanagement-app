using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleetmanagement_app_DAL.Entities
{
    public class Koppeling
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid KoppelingsId { get; set; }

        [Required]
        public string Rijksregisternummer { get; set; }

        public virtual Bestuurder Bestuurder { get; set; }

        public virtual string Chassisnummer { get; set; }
        public virtual Voertuig Voertuig { get; set; }

        public string Kaartnummer { get; set; }
        public virtual Tankkaart Tankkaart { get; set; }
    }
}