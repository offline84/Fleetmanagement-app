using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleetmanagement_app_DAL.Entities
{
    public class Categorie
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = true)]
        public string TypeWagen { get; set; }

        public virtual ICollection<Voertuig> Voertuigen { get; set; }
    }
}