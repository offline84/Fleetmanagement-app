using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fleetmanagement_app_DAL.Entities
{
    public class Tankkaart
    {
        public Tankkaart(string kaartnummer, DateTime geldigheidsDatum)
        {
            Kaartnummer = kaartnummer;
            GeldigheidsDatum = geldigheidsDatum;
        }

        [Key]
        public string Kaartnummer { get; set; }

        public DateTime GeldigheidsDatum { get; set; }

        [MaxLength(8), MinLength(4)]
        public int Pincode { get; set; }

        public virtual ICollection<ToewijzingBrandstofTankkaart> MogelijkeBrandstoffen { get; set; }

        [NotMapped()]
        public virtual ICollection<Brandstof> Brandstoffen { get; set; }
            = new List<Brandstof>();

        public virtual Koppeling Koppeling { get; set; }

        public bool IsGeblokkeerd { get; set; }

        public bool IsGearchiveerd { get; set; }

        public DateTime LaatstGeupdate { get; set; }
    }
}