using System;
using System.Collections.Generic;

namespace FleetmanagementApp.BUL.Models
{
    public class BrandstofModel
    {
        public Guid Id { get; set; }
        public string TypeBrandstof { get; set; }
        public virtual ICollection<VoertuigModel> Voertuigen { get; set; }
        public virtual ICollection<ToewijzingBrandstofTankkaartModel> Toewijzingen { get; set; }
    }
}