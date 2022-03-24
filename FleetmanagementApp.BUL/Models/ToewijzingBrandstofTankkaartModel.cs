using System;
using System.ComponentModel.DataAnnotations;
namespace FleetmanagementApp.BUL.Models
{
    public class ToewijzingBrandstofTankkaartModel
    {
        public Guid Id { get; set; }
        public Guid BrandstofId { get; set; }
        public string Tankkaartnummer { get; set; }
        public virtual TankkaartModel Tankkaart { get; set; }
        public virtual BrandstofModel Brandstof { get; set; }
    }
}
