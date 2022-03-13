using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Bestuurder
    {
        [Key]
        public string PersoneelsId {get; set;}

        [MaxLength(50)]
        public string Naam { get; set; }

        [MaxLength(50)]
        public string Achternaam { get; set; }

        public Adres Adres {get; set;}

        [Timestamp]
        public DateTime GeboorteDatum {get; set;}

        [Required]
        [StringLength(11)]
        public string Rijksregisternummer {get; set;}

        public ICollection<Rijbewijs> Rijbewijzen {get; set;}
            = new List<Rijbewijs>();

        public Voertuig Voertuig {get; set;}

        public string TankkaartId {get; set;}

        public Tankkaart? Tankkaart {get; set;}

        public bool IsGearchiveerd {get; set;}

        [Timestamp]
        public DateTime LaatstGeupdate {get; set;}
    }
    public class Adres
    {
        public string Straat {get; set;}
        public int Huisnummer {get; set;}
        public string Stad {get; set;}
        [MaxLength(50)]
        public int Postcode {get; set;}
    }
}
