using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class ToewijzingRijbewijsBestuurder
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Rijksregisternummer { get; set; }

        public virtual Bestuurder Bestuurder { get; set; }

        [Required]
        public Guid RijbewijsId { get; set; }

        public virtual Rijbewijs Rijbewijs { get; set; }


    }
}
