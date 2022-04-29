namespace FleetManagement_app_PL.ViewModel
{
    public class AdresViewingDto
    {
        public string Straat { get; set; }

        public int Huisnummer { get; set; }

        public string Stad { get; set; }

        public int Postcode { get; set; }

        public BestuurderViewingDto Bestuurder { get; set; }
    }
}
