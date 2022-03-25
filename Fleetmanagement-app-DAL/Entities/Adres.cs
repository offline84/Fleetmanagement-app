using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Fleetmanagement_app_Groep1.Entities
{
    [Owned]
    public class Adres
    {
        public string Straat { get; set; }

        public int Huisnummer { get; set; }

        public string Stad { get; set; }

        [MaxLength(50)]
        public int Postcode { get; set; }

        public Bestuurder Bestuurder { get; set; }
    }
}