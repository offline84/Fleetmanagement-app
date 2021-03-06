using Fleetmanagement_app_DAL.Entities;
using System;
using System.Collections.Generic;

namespace FleetManagement_app_PL.ViewModel
{
    public class BestuurderViewingDto
    {
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

        public ICollection<ToewijzingRijbewijsBestuurderViewingDto> ToewijzingenRijbewijs { get; set; }

        public KoppelingForViewingDto Koppeling { get; set; }
    }
}
