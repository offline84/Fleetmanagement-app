using System;
using System.ComponentModel.DataAnnotations;

namespace Fleetmanagement_app_DAL.Entities
{
    public class Voertuig
    {
        /// <summary>
        /// Voertuig kan enkel door de Builder aangemaakt worden, hierdoor voorkomen we dat ongeldige data opgevangen wordt
        /// en niet leiden zal tot onvolledige gegevens of fouten van het schrijven naar de database.
        /// </summary>
        protected internal Voertuig()
        {
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

        [MaxLength(2)]
        public int AantalDeuren { get; set; }

        public virtual Koppeling Koppeling { get; set; }

        public Guid StatusId { get; set; }

        public virtual Status Status { get; set; }

        public bool IsGearchiveerd { get; set; }

        public DateTime LaatstGeupdate { get; set; }
    }
}