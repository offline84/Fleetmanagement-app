using System;
using System.Collections.Generic;

namespace FleetmanagementApp.BUL.Models
{
    public class BestuurderModel
    {
        public BestuurderModel(string naam, string achternaam, DateTime geboorteDatum, string rijksregisternummer)
        {
            Naam = naam;
            Achternaam = achternaam;
            GeboorteDatum = geboorteDatum;
            Rijksregisternummer = rijksregisternummer;
        }

        public string Rijksregisternummer { get; set; }
        public string Naam { get; set; }
        public string Achternaam { get; set; }
        public bool IsGearchiveerd { get; set; }
        public DateTime GeboorteDatum { get; set; }
        public DateTime LaatstGeupdate { get; set; }
        public virtual AdresModel Adres { get; set; }
        public virtual KoppelingModel Koppeling { get; set; }
        public virtual ICollection<ToewijzingRijbewijsBestuurderModel> ToewijzingenRijbewijs { get; set; }
        public virtual ICollection<RijbewijsModel> Rijbewijzen { get; private set; }

    }


}
