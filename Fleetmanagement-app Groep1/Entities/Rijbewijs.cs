using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Rijbewijs
    {
        [Key]
        public Guid Id {get; set;}

        [Required]
        [MaxLength(4)]
        public string TypeRijbewijs {get; set;}
    }
}
