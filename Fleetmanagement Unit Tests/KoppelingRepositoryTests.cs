using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Fleetmanagement_app_Groep1.Helpers;
using Fleetmanagement_app_BLL.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using System.Collections.Generic;
using Fleetmanagement_app_DAL.Builders;
using Microsoft.EntityFrameworkCore;

namespace Fleetmanagement_Unit_Tests
{
    class KoppelingRepositoryTests
    {
        private static FleetmanagerContext _context = new FleetmanagerContext(DbContextHelper.GetDbContextOptions("Testing"));
        private ILoggerFactory _loggerfactory = new LoggerFactory();
        private KoppelingRepository _repo;

        public KoppelingRepositoryTests()
        {
            _repo = new KoppelingRepository(_context, _loggerfactory.CreateLogger("Koppelingestlogs"));
        }

        internal Bestuurder GetBestuurder1()
        {
            var bestuurder = new Bestuurder("Maiko", "Samyn", new DateTime(17 / 06 / 1984), "84061703993");

            Adres adres = new Adres()
            {
                Huisnummer = 61,
                Straat = "Kouter",
                Postcode = 9000,
                Stad = "Gent"
            };

            bestuurder.Adres = adres;
            Rijbewijs r = new Rijbewijs() { TypeRijbewijs = "X++" };

            bestuurder.Rijbewijzen.Add(r);

            return bestuurder;
        }

        internal Tankkaart GetTankkaart1()
        {
            var tankkaart = new Tankkaart("123456789", DateTime.Now);
            tankkaart.Pincode = 1234;
            var toewijzingen = new List<ToewijzingBrandstofTankkaart>();

            ToewijzingBrandstofTankkaart toewijzing1 = new ToewijzingBrandstofTankkaart()
            {
                Tankkaart = tankkaart,
                Brandstof = _context.Brandstof.Where(b => b.TypeBrandstof == "euro 95").Single()
            };
            ToewijzingBrandstofTankkaart toewijzing2 = new ToewijzingBrandstofTankkaart()
            {
                Tankkaart = tankkaart,
                Brandstof = _context.Brandstof.Where(b => b.TypeBrandstof == "euro 98").Single()
            };
            toewijzingen.Add(toewijzing1);
            toewijzingen.Add(toewijzing2);
            tankkaart.MogelijkeBrandstoffen = toewijzingen;

            return tankkaart;
        }

        internal Voertuigbuilder GetVoertuig1()
        {
            var brandstof = _context.Brandstof.FirstOrDefault();
            var status = _context.Status.Where(s => s.Staat == "in bedrijf").FirstOrDefault();
            var categorie = _context.Categorie.FirstOrDefault();

            var voertuig = new Voertuigbuilder()
            {
                Chassisnummer = "VF37BRFVE12345678",
                Merk = "Ford",
                Nummerplaat = "VNG746",
                Model = "Cobra",
                Bouwjaar = 1987,
                AantalDeuren = 3,
                Kleur = "midnight pink",
                Categorie = categorie,
                Status = status,
                Brandstof = brandstof
            };
        }


        internal void Cleanup()
        {
            var tankkaarten = _context.Tankkaarten.ToList();
            var toewijzingen = _context.ToewijzingBrandstofTankkaarten.ToList();
            var bestuurders = _context.Bestuurders.ToList();
            var koppelingen = _context.Koppelingen.ToList();

            _context.Tankkaarten.RemoveRange(tankkaarten);
            _context.ToewijzingBrandstofTankkaarten.RemoveRange(toewijzingen);
            _context.Bestuurders.RemoveRange(bestuurders);
            _context.Koppelingen.RemoveRange(koppelingen);
            _context.SaveChanges();
        }

        public async Task KoppelingWordtAangemaaktBijAddBestuurder()
        {
            Cleanup();
            var bestuurder = GetBestuurder1();
            var bestuurderRRN = bestuurder.Rijksregisternummer;
            _context.Bestuurders.Add(bestuurder);
            _context.SaveChanges();

            var koppelingByBestuurder = _repo.GetByBestuurder(bestuurderRRN);

            Assert.NotNull(koppelingByBestuurder);
        }

        public async Task GetByBestuurder_Werkt()
        {
            Cleanup();
            var bestuurder = GetBestuurder1();
            var bestuurderRRN = bestuurder.Rijksregisternummer;
            _context.Bestuurders.Add(bestuurder);
            _context.SaveChanges();

            var koppelingByBestuurder = _repo.GetByBestuurder(bestuurderRRN).Result;
            var koppelingByBestuurderViaDB = await _context.Koppelingen.Where(k => k.Rijksregisternummer == bestuurderRRN).FirstOrDefaultAsync();

            Assert.Equal(koppelingByBestuurder, koppelingByBestuurderViaDB);
        }

        public async Task GetByTankkaart_Werkt()
        {
            Cleanup();
            var bestuurder = GetBestuurder1();
            var bestuurderRRN = bestuurder.Rijksregisternummer;
            _context.Bestuurders.Add(bestuurder);
            var tankkaart = GetTankkaart1();
            var kaartnummer = tankkaart.Kaartnummer;
            _context.Tankkaarten.Add(tankkaart);
            _context.SaveChanges();

            await _repo.KoppelAanTankkaart(bestuurderRRN, kaartnummer);

            var koppelingByTankkaart = _repo.GetByTankkaart(kaartnummer).Result;
            var koppelingByTankkaartViaDB = await _context.Koppelingen.Where(k => k.Kaartnummer == kaartnummer).FirstOrDefaultAsync();

            Assert.Equal(koppelingByTankkaart, koppelingByTankkaartViaDB);
        }

        public async Task KoppelAanTankkaart_KoppelingWerkt()
        {
            Cleanup();
            var bestuurder = GetBestuurder1();
            var bestuurderRRN = bestuurder.Rijksregisternummer;
            _context.Bestuurders.Add(bestuurder);
            var tankkaart = GetTankkaart1();
            var kaartnummer = tankkaart.Kaartnummer;
            _context.Tankkaarten.Add(tankkaart);
            _context.SaveChanges();

            await _repo.KoppelAanTankkaart(bestuurderRRN, kaartnummer);

            var koppelingByBestuurder = _repo.GetByBestuurder(bestuurderRRN);
            var koppelingByTankkaart = _repo.GetByTankkaart(kaartnummer);

            Assert.Equal(koppelingByBestuurder, koppelingByTankkaart);
        }

        public async Task KoppelAanVoertuig_KoppelingWerkt()
        {
            Cleanup();
            var bestuurder = GetBestuurder1();
            var bestuurderRRN = bestuurder.Rijksregisternummer;
            _context.Bestuurders.Add(bestuurder);
            var builder = GetVoertuig1();
            var voertuig = builder.Build();
            var chassisnummer = voertuig.Chassisnummer;
            _context.Voertuigen.Add(voertuig);
            _context.SaveChanges();

            await _repo.KoppelAanVoertuig(bestuurderRRN, chassisnummer);

            var koppelingByBestuurder = _repo.GetByBestuurder(bestuurderRRN);
            var koppelingByVoertuig = _repo.GetByvoertuig(chassisnummer);

            Assert.Equal(koppelingByBestuurder, koppelingByVoertuig);
        }

        public async Task GetByVoertuig_Werkt()
        {
            Cleanup();
            var bestuurder = GetBestuurder1();
            var bestuurderRRN = bestuurder.Rijksregisternummer;
            _context.Bestuurders.Add(bestuurder);
            var builder = GetVoertuig1();
            var voertuig = builder.Build();
            var chassisnummer = voertuig.Chassisnummer;
            _context.Voertuigen.Add(voertuig);
            _context.SaveChanges();

            await _repo.KoppelAanVoertuig(bestuurderRRN, chassisnummer);

            var koppelingByVoertuig = _repo.GetByvoertuig(chassisnummer).Result;
            var koppelingByVoertuigViaDB = await _context.Koppelingen.Where(k => k.Chassisnummer == chassisnummer).FirstOrDefaultAsync();


            Assert.Equal(koppelingByVoertuig, koppelingByVoertuigViaDB);
        }

        public async Task KoppelLosVanTankkaart_LosKoppelengWerkt()
        {
            throw new NotImplementedException();
        }
        public async Task KoppelLosVanVoertuig_LosKoppelengWerkt()
        {
            throw new NotImplementedException();
        }

    }
}
