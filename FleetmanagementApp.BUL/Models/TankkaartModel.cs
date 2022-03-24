using System;
using System.Collections.Generic;

namespace FleetmanagementApp.BUL.Models
{
    public class TankkaartModel
    {

        public TankkaartModel(string kaartnummer, DateTime geldigheidsDatum)
        {
            Kaartnummer = kaartnummer;
            GeldigheidsDatum = geldigheidsDatum;
        }

        public string Kaartnummer { get; set; }
        public int Pincode { get; set; }
        public bool IsGeblokkeerd { get; set; }
        public bool IsGearchiveerd { get; set; }
        public DateTime GeldigheidsDatum { get; set; }
        public DateTime LaatstGeupdate { get; set; }
        public virtual KoppelingModel Koppeling { get; set; }

        public virtual ICollection<ToewijzingBrandstofTankkaartModel> MogelijkeBrandstoffen { get; set; }
     = new List<ToewijzingBrandstofTankkaartModel>();

    }
}