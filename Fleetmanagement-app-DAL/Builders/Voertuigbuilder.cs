using Fleetmanagement_app_Groep1.Entities;
using FleetmanagementApp.BUL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Text;

namespace FleetmanagementApp.BUL.Repository.Builders
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
          
            if(!IsValid()) 
                throw new InvalidOperationException(Error());

            if(Nummerplaat.Trim() == "" && Status != null && Status.Staat != "in aankoop")
                throw new InvalidOperationException("Voertuigen zonder nummerplaat kunnen enkel als deze de status -in aankoop- hebben");



            var result = new Voertuig
            {
                Chassisnummer = this.Chassisnummer.ToUpper(),
                Merk = this.Merk,
                Model = this.Model,
                Nummerplaat = this.Nummerplaat,
                Brandstof = this.Brandstof,
                BrandstofId = this.Brandstof.Id,
                Categorie = this.Categorie,
                CategorieId = this.Categorie.Id,
                AantalDeuren = AantalDeuren.Value,
                Bouwjaar = this.Bouwjaar.Value,
                Kleur = this.Kleur,
                Status = this.Status,
                StatusId = this.Status.Id,
                LaatstGeupdate = DateTime.Now,
                Koppeling = this.Koppeling,
                
            };
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
            if (Chassisnummer.Trim() == "") errormessage.AppendLine("Model ontbreekt");
            if (Merk.Trim() == "") errormessage.AppendLine("Merk ontbreekt");
            if(Chassisnummer.Trim() == "") errormessage.AppendLine("Chassisnummer ontbreekt");
            if(Categorie == null) errormessage.AppendLine("Categorie / Type wagen ontbreekt");
            if(Brandstof == null) errormessage.AppendLine("Type brandstof ontbreekt");

            return errormessage.ToString();
            
        }
    }
}
