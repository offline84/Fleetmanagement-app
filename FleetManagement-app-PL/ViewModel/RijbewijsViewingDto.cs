using Fleetmanagement_app_DAL.Entities;
using System;
using System.Collections.Generic;

namespace FleetManagement_app_PL.ViewModel
{
    public class RijbewijsViewingDto
    {
        public Guid Id { get; set; }

        public string TypeRijbewijs { get; set; }

        public virtual ICollection<ToewijzingRijbewijsBestuurder> ToewijzingenBestuurder { get; set; } 
    }
}
