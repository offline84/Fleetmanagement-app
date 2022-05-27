using System;
using System.Collections.Generic;
using System.Text;

namespace FleetManagement_app_PL.ViewModel
{
    public class KoppelingForViewingDto
    {
         public Guid KoppelingsId { get; set; }

        public string Rijksregisternummer { get; set; }

        public virtual string Chassisnummer { get; set; }

        public string Kaartnummer { get; set; }
    }
}
