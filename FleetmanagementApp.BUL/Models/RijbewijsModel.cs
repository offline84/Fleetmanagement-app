using System;
using System.Collections.Generic;

namespace FleetmanagementApp.BUL.Models
{
    public class RijbewijsModel
    {
        public Guid Id { get; set; }
        public string TypeRijbewijs { get; set; }
        public ICollection<ToewijzingRijbewijsBestuurderModel> ToewijzingenBestuurder { get; set; }
    }
}
