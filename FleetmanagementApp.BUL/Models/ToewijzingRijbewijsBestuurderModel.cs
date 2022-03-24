using System;

namespace FleetmanagementApp.BUL.Models
{
    public class ToewijzingRijbewijsBestuurderModel
    {
        public Guid Id { get; set; }
        public string Rijksregisternummer { get; set; }
        public Guid RijbewijsId { get; set; }
        public virtual BestuurderModel Bestuurder { get; set; }
        public virtual RijbewijsModel Rijbewijs { get; set; }
    }
}
