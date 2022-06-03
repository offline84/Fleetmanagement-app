using Fleetmanagement_app_DAL.Entities;
using System;

namespace FleetManagement_app_PL.ViewModel
{
    public class VoertuigForViewingDto
    {
        public string Chassisnummer { get; set; }

        public string Merk { get; set; }

        public string Model { get; set; }

        public int Bouwjaar { get; set; }

        public string Nummerplaat { get; set; }

        public BrandstofForViewingDto Brandstof { get; set; }

        public CategorieForViewingDto Categorie { get; set; }

        public string Kleur { get; set; }

        public int AantalDeuren { get; set; }

        public StatusForViewingDto Status { get; set; }

        public KoppelingForViewingDto Koppeling { get; set; }

        public bool IsGearchiveerd { get; set; }

        public DateTime LaatstGeupdate { get; set; }
    }
}