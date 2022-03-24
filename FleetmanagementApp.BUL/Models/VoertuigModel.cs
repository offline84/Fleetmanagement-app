using System;
using System.ComponentModel.DataAnnotations;

namespace FleetmanagementApp.BUL.Models
{
    public class VoertuigModel
    {
        public VoertuigModel(string chassisnummer, string merk, string model, Guid brandstofId, Guid categorieId)
        {
            Chassisnummer = chassisnummer;
            Merk = merk;
            Model = model;
            CategorieId = categorieId;
            BrandstofId = brandstofId;
        }

        public string Chassisnummer { get; set; }
        public string Merk { get; set; }
        public string Model { get; set; }
        public int Bouwjaar { get; set; }
        public string Nummerplaat { get; set; }
        public Guid BrandstofId { get; set; }
        public Guid CategorieId { get; set; }
        public string Kleur { get; set; }
        public int AantalDeuren { get; set; }
        public Guid StatusId { get; set; }
        public bool IsGearchiveerd { get; set; }
        public DateTime LaatstGeupdate { get; set; }
        public virtual BrandstofModel Brandstof { get; set; }
        public virtual CategorieModel Categorie { get; set; }
        public virtual KoppelingModel Koppeling { get; set; }
        public virtual StatusModel Status { get; set; }
    }
}