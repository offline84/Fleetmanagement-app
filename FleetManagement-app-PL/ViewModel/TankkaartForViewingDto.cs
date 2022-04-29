﻿using Fleetmanagement_app_DAL.Entities;
using System;

namespace FleetManagement_app_PL.ViewModel
{
    public class TankkaartForViewingDto
    {

        public TankkaartForViewingDto(string kaartnummer, DateTime geldigheidsDatum)
        {
            Kaartnummer = kaartnummer;
            GeldigheidsDatum = geldigheidsDatum;
        }

        public string Kaartnummer { get; set; }

        public DateTime GeldigheidsDatum { get; set; }

        public int Pincode { get; set; }

        public virtual Koppeling Koppeling { get; set; }

        public bool IsGeblokkeerd { get; set; }

        public bool IsGearchiveerd { get; set; }

        public DateTime LaatstGeupdate { get; set; }
    }
}
