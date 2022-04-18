using System;

namespace FleetManagement_app_PL.ViewModel
{
    public class BestuurderViewModel
    {
        public BestuurderViewModel(string naam, string achternaam, DateTime geboorteDatum, string rijksregisternummer)
        {
            Naam = naam;
            Achternaam = achternaam;
            GeboorteDatum = geboorteDatum;
            Rijksregisternummer = rijksregisternummer;
        }

        public BestuurderViewModel()
        {

        }

        public string Rijksregisternummer { get; set; }

        public string Naam { get; set; }

        public string Achternaam { get; set; }

        public DateTime GeboorteDatum { get; set; }

        public bool IsGearchiveerd { get; set; }

        public DateTime LaatstGeupdate { get; set; }
    }
}
