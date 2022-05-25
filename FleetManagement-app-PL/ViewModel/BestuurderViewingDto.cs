using System;
using System.Collections.Generic;

namespace FleetManagement_app_PL.ViewModel
{
    public class BestuurderViewingDto
    {
        public BestuurderViewingDto(string naam, string achternaam, DateTime geboorteDatum, string rijksregisternummer)
        {
            Naam = naam;
            Achternaam = achternaam;
            GeboorteDatum = geboorteDatum;
            Rijksregisternummer = rijksregisternummer;
        }

        public BestuurderViewingDto()
        {
        }

        public string Rijksregisternummer { get; set; }

        public string Naam { get; set; }

        public string Achternaam { get; set; }

        public DateTime GeboorteDatum { get; set; }

        public bool IsGearchiveerd { get; set; }

        public DateTime LaatstGeupdate { get; set; }

        public AdresViewingDto Adres { get; set; }

        //public virtual Koppeling Koppeling { get; set; }

        //public ICollection<RijbewijsViewingDto> Rijbewijzen { get; set; }

        public ICollection<ToewijzingRijbewijsBestuurderViewingDto> ToewijzingenRijbewijs { get; set; }
    }
}
