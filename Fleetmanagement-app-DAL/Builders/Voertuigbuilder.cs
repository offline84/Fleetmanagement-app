
using Fleetmanagement_app_DAL.Entities;
using System;
using System.Text;

namespace Fleetmanagement_app_DAL.Builders
{
    public class Voertuigbuilder
    {
        private readonly IVoertuigBuilderRepository _repo;

        public Voertuigbuilder(IVoertuigBuilderRepository repo)
        {
            this._repo = repo;
        }

        public string Chassisnummer { get; set; }

        public string Merk { get; set; }

        public string Model { get; set; }

        public string Nummerplaat { get; set; }

        public Brandstof Brandstof { get; set; }

        public Categorie Categorie { get; set; }

        public int? AantalDeuren { get; set;}
        public int? Bouwjaar { get; set; }
        public string Kleur { get; set; }
        public Status Status {get; set;}
        public Koppeling Koppeling {get; set;}

        public Voertuig Build()
        {

            if(Nummerplaat.Trim()== "")
                    if(Status != null && Status.Staat != "in aankoop")
                        throw new InvalidOperationException("Geen nummerplaat kan enkel als de status van het voertuig -in aankoop- is. \n" + Error());
                    else if(Status == null)
                        throw new InvalidOperationException("Geen nummerplaat kan enkel als de status van het voertuig -in aankoop- is. \n" + Error());


            if(!IsValid()) 
                throw new InvalidOperationException(Error());


            var result = new Voertuig
            {
                Chassisnummer = this.Chassisnummer.ToUpper().Trim(),
                Merk = this.Merk.Trim(),
                Model = this.Model.Trim(),
                Nummerplaat = this.Nummerplaat.Trim(),
                Brandstof = this.Brandstof,
                BrandstofId = this.Brandstof.Id,
                Categorie = this.Categorie,
                CategorieId = this.Categorie.Id,
                AantalDeuren = AantalDeuren.Value,
                Bouwjaar = this.Bouwjaar.Value,
                Kleur = this.Kleur,
                LaatstGeupdate = DateTime.Now,
                Koppeling = this.Koppeling,
                
            };

            if(Status != null)
            {
                result.Status = this.Status;
                result.StatusId = this.Status.Id;
            }

            return result;
        }

        public bool IsValid()
        {
            return Chassisnummer.Trim() != ""
                && Merk.Trim() != ""
                && Model.Trim() != ""
                && Categorie != null
                && Brandstof != null;
                
        }

        private string Error()
        {
            var errormessage = new StringBuilder();
            if (Chassisnummer.Trim() == "") errormessage.AppendLine("Model ontbreekt. \n");
            if (Merk.Trim() == "") errormessage.AppendLine("Merk ontbreekt. \n");
            if(Chassisnummer.Trim() == "") errormessage.AppendLine("Chassisnummer ontbreekt. \n");
            if(Categorie == null) errormessage.AppendLine("Categorie / Type wagen ontbreekt. \n");
            if(Brandstof == null) errormessage.AppendLine("Type brandstof ontbreekt. \n");

            return errormessage.ToString();
            
        }
    }
}
