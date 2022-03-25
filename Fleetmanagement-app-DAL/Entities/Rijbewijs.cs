using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Rijbewijs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id {get; set;}

        [Required]
        public string TypeRijbewijs {get; set;}

        public ICollection<ToewijzingRijbewijsBestuurder> ToewijzingenBestuurder {get; set;}
    }
}
