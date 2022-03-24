using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FleetmanagementApp.BUL.Models
{
    public class StatusModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Staat { get; set; }

        public virtual ICollection<VoertuigModel> Voertuigen { get; set; }
    }
}