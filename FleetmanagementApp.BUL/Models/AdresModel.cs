namespace FleetmanagementApp.BUL.Models
{
    public class AdresModel
    {
        public string Straat { get; set; }
        public int Huisnummer { get; set; }
        public string Stad { get; set; }
        public int Postcode { get; set; }
        public BestuurderModel Bestuurder { get; set; }
    }
}