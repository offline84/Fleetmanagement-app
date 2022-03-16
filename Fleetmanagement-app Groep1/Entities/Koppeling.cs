using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Koppeling
    {
        [Key]
        public Guid KoppelingsId {get; set;}

        [Required]
        public string PersoneelsId {get; set;}
        public virtual Bestuurder Bestuurder {get; set;}

        public virtual string VoertuigId {get; set;}
        public virtual Voertuig Voertuig {get; set;}

        public string TankkaartId { get; set; }
        public virtual Tankkaart Tankkaart {get; set;}
    }
}
