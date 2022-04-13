using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Timestamp]
        public DateTime GeldigheidsDatum { get; set; }

        [MaxLength(8), MinLength(4)]
        public int Pincode { get; set; }

        public virtual ICollection<ToewijzingBrandstofTankkaart> MogelijkeBrandstoffen { get; set; }
            = new List<ToewijzingBrandstofTankkaart>();

        public virtual Koppeling Koppeling { get; set; }

        public bool IsGeblokkeerd { get; set; }

        public bool IsGearchiveerd { get; set; }

        [Timestamp]
        public DateTime LaatstGeupdate { get; set; }
    }
}