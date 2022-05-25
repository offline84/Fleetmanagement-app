using System;
using System.Collections.Generic;

namespace FleetManagement_app_PL.ViewModel
{
    public class TankkaartForViewingDto
    {
        public TankkaartForViewingDto(){}

        public string Kaartnummer { get; set; }

        public DateTime GeldigheidsDatum { get; set; }

        public int Pincode { get; set; }
        public bool IsGeblokkeerd { get; set; }
        public bool IsGearchiveerd { get; set; }
        public  ICollection<ToewijzingBrandstofTankkaartForViewingDto> MogelijkeBrandstoffen { get; set; }
        
    }
}
