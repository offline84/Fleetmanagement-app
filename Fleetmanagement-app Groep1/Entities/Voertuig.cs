using System;
using System.ComponentModel.DataAnnotations;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Voertuig
    {
        [Key]
        public string Chassisnummer {get; set;}

        [MaxLength(50)]
        public string Merk {get; set;}

        [MaxLength(50)]
        public string Model {get; set;}

        [MaxLength(50)]
        public string Nummerplaat {get; set;}

        public Brandstof Brandstof {get; set;}

       // public Categorie Categorie {get; set;}

        [MaxLength(50)]
        public string Kleur {get; set;}
       
        [MaxLength(2)]
        public int AantalDeuren {get; set;}

       // public Status Status {get; set;}

        public Bestuurder bestuurder {get; set;}

        public bool IsGearchiveerd {get; set;}

        [Timestamp]
        public DateTime LaatstGeupdate {get; set;}

        public class Status
        {
            public string Staat {get; set;}
        }
        public class Categorie
        {
            public string TypeWagen {get; set;}
        }
    }
}
    