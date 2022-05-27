using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleetmanagement_app_DAL.Entities
{
    public class Bestuurder
    {
        // Rijbewijzen zijn verplicht bij aanmaken Bestuurder , maar Objecten kunnen niet doorgegeven worden in ctor aangezien zij niet gebonden kunnen worden aan de DB
        public Bestuurder(string naam, string achternaam, DateTime geboorteDatum, string rijksregisternummer)
        {
            Naam = naam;
            Achternaam = achternaam;
            GeboorteDatum = geboorteDatum;
            Rijksregisternummer = rijksregisternummer;
        }

        [Key]
        [StringLength(11)]
        public string Rijksregisternummer { get; set; }

        [MaxLength(50)]
        public string Naam { get; set; }

        [MaxLength(50)]
        public string Achternaam { get; set; }

        public virtual Adres Adres { get; set; }

        public DateTime GeboorteDatum { get; set; }

        public virtual ICollection<ToewijzingRijbewijsBestuurder> ToewijzingenRijbewijs { get; set; }

        /*[NotMapped()]
        public virtual ICollection<Rijbewijs> Rijbewijzen { get; set; } */

        public virtual Koppeling Koppeling { get; set; }

        public bool IsGearchiveerd { get; set; }

        public DateTime LaatstGeupdate { get; set; }
    }
}