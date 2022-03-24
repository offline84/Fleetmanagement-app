using System;

namespace FleetmanagementApp.BUL.Models
{
    public class KoppelingModel
    {
        public Guid KoppelingsId { get; set; }
        public string Rijksregisternummer { get; set; }
        public virtual string Chassisnummer { get; set; }
        public string Kaartnummer { get; set; }
        public virtual BestuurderModel Bestuurder { get; set; }
        public virtual VoertuigModel Voertuig { get; set; }
        public virtual TankkaartModel Tankkaart { get; set; }
    }
}
