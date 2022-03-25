using System;
using System.ComponentModel.DataAnnotations;

namespace Fleetmanagement_app_Groep1.Entities
{
    public class Voertuig
    {
        public Voertuig(string chassisnummer, string merk, string model, Guid brandstofId, Guid categorieId)
        {
            Chassisnummer = chassisnummer;
            Merk = merk;
            Model = model;
            CategorieId = categorieId;
            BrandstofId = brandstofId;
        }

        [Key]
        public string Chassisnummer { get; set; }

        [Required]
        [MaxLength(50)]
        public string Merk { get; set; }

        [Required]
        [MaxLength(50)]
        public string Model { get; set; }

        public int Bouwjaar { get; set; }

        [MaxLength(50)]
        public string Nummerplaat { get; set; }

        [Required]
        public Guid BrandstofId { get; set; }

        public virtual Brandstof Brandstof { get; set; }

        [Required]
        public Guid CategorieId { get; set; }

        public virtual Categorie Categorie { get; set; }

        [MaxLength(50)]
        public string Kleur { get; set; }

        [Required]
        [MaxLength(2)]
        public int AantalDeuren { get; set; }

        public virtual Koppeling Koppeling { get; set; }

        public Guid StatusId { get; set; }

        public virtual Status Status { get; set; }

        public bool IsGearchiveerd { get; set; }

        [Timestamp]
        public DateTime LaatstGeupdate { get; set; }
    }
}