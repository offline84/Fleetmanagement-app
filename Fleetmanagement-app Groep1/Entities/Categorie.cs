using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Categorie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string TypeWagen { get; set; }

        public virtual ICollection<Voertuig> Voertuigen { get; set; }
    }
}