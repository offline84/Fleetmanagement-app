using System;
using System.Collections.Generic;

namespace FleetmanagementApp.BUL.Models
{
    public class CategorieModel
    {
        public Guid Id { get; set; }
        public string TypeWagen { get; set; }
        public virtual ICollection<VoertuigModel> Voertuigen { get; set; }
    }
}