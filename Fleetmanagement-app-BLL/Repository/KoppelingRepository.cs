using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public class KoppelingRepository : GenericRepository<Koppeling>, IKoppelingRepository
    {
        public KoppelingRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        public async Task<bool> CreateKoppeling(string bestuurderRRN)
        {
            if (BestuurderBestaatNiet(bestuurderRRN))
            {
                _logger.LogWarning("Koppeling aanmaken: bestuurder bestaan niet");
                return false;
            }

            var koppeling = new Koppeling();
            koppeling.Rijksregisternummer = bestuurderRRN;

            try
            {
                await _context.Koppelingen.AddAsync(koppeling);
                _logger.LogWarning("Koppeling aanmaken: Koppeling aangemaakt");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Koppeling aanmaken Er is iets foutgelopen bij het aanmaken van de koppeling in de DB", e);
                return false;
            }
        }

        public async Task<bool> KoppelAanTankkaart(string bestuurderRRN, string kaartnummer)
        {
            if (KoppelingBestuurderBestaatNiet(bestuurderRRN) | TankkaartBestaatNiet(kaartnummer))
            {
                _logger.LogWarning("Koppeling: Koppeling, bestuurder of tankkaart bestaan niet");
                return false;
            }

            var koppeling = _dbSet.Where(k => k.Rijksregisternummer == bestuurderRRN).FirstOrDefault();
            koppeling.Kaartnummer = kaartnummer;

            try
            {
                _dbSet.Update(koppeling);
                _logger.LogWarning("Koppeling: Koppeling tankkaart succesvol");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Koppeling: Kon tankkaart niet koppelen aan bestuurder", e);
                return false;
            }
        }

        public async Task<bool> KoppelAanVoertuig(string bestuurderRRN, string chassisnummer)
        {
            if (KoppelingBestuurderBestaatNiet(bestuurderRRN) | VoertuigBestaatNiet(chassisnummer))
            {
                _logger.LogWarning("Koppeling: Koppeling, bestuurder of voertuig bestaan niet");
                return false;
            }

            var koppeling = _dbSet.Where(k => k.Rijksregisternummer == bestuurderRRN).FirstOrDefault();
            koppeling.Chassisnummer = chassisnummer;

            try
            {
                _dbSet.Update(koppeling);
                _logger.LogWarning("Koppeling: Koppeling voertuig succesvol");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Koppeling: Kon voertuig niet koppelen aan bestuurder", e);
                return false;
            }
        }

        public async Task<bool> KoppelLosVanTankkaart(string kaartnummer)
        {
            if (KoppelingTankkaartBestaatNiet(kaartnummer))
            {
                _logger.LogWarning("Los Koppelen: Koppeling met Tankkaart bestaan niet");
                return false;
            }
            var koppeling = _dbSet.Where(k => k.Kaartnummer == kaartnummer).FirstOrDefault();
            koppeling.Kaartnummer = null;
            try
            {
                _dbSet.Update(koppeling);
                _logger.LogWarning("Los Koppelen: Tankkaart losgekoppeld van bestuuurder");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Los Koppelen: Kon Tankkaart niet los koppelen van de bestuurder", e);
                return false;
            }
        }

        public async Task<bool> KoppelLosVanVoertuig(string chassisnummer)
        {
            if (KoppelingVoertuigBestaatNiet(chassisnummer))
            {
                _logger.LogWarning("Los Koppelen: Koppeling met Voertuig bestaan niet");
                return false;
            }
            var koppeling = _dbSet.Where(k => k.Chassisnummer == chassisnummer).FirstOrDefault();
            koppeling.Chassisnummer = null;

            try
            {
                _dbSet.Update(koppeling);
                _logger.LogWarning("Los Koppelen: Voertuig losgekoppeld van bestuuurder");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Los Koppelen: Kon Voertuig niet los gekoppelen van de bestuurder", e);
                return false;
            }
        }

        public async Task<bool> KoppelLosVanBestuurder(string bestuurderRRN)
        {
            if (KoppelingBestuurderBestaatNiet(bestuurderRRN))
            {
                _logger.LogWarning("Los Koppelen: Koppeling bestuurder bestaan niet");
                return false;
            }
            var koppeling = _dbSet.Where(k => k.Rijksregisternummer == bestuurderRRN).FirstOrDefault();
            koppeling.Kaartnummer = null;
            koppeling.Chassisnummer = null;

            try
            {
                _dbSet.Update(koppeling);
                _logger.LogWarning("Los Koppelen: Bestuurder losgekoppeld van tankkaart en voertuig");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Los Koppelen: Kon tankkaart niet los koppelen van de bestuurder", e);
                return false;
            }
        }


        public async Task<Koppeling> GetByBestuurder(string bestuurderRRN)
        {
            return await _dbSet.Where(k => k.Rijksregisternummer == bestuurderRRN).FirstOrDefaultAsync();
        }

        public async Task<Koppeling> GetByTankkaart(string kaartnummer)
        {
            return await _dbSet.Where(k => k.Kaartnummer == kaartnummer).FirstOrDefaultAsync();
        }

        public async Task<Koppeling> GetByvoertuig(string chassisnummer)
        {
            return await _dbSet.Where(k => k.Chassisnummer == chassisnummer).FirstOrDefaultAsync();
        }

        private bool KoppelingBestuurderBestaatNiet(string bestuurderRRN)
        {
            if (_dbSet.Where(k => k.Rijksregisternummer == bestuurderRRN).Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool KoppelingTankkaartBestaatNiet(string kaartnummer)
        {
            if (_dbSet.Where(k => k.Kaartnummer == kaartnummer).Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool KoppelingVoertuigBestaatNiet(string chassisnummer)
        {
            if (_dbSet.Where(k => k.Chassisnummer == chassisnummer).Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool BestuurderBestaatNiet(string bestuurderRRN)
        {
            if (_context.Bestuurders.Where(b => b.Rijksregisternummer == bestuurderRRN).Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool TankkaartBestaatNiet(string kaartnummer)
        {
            if (_context.Tankkaarten.Where(t => t.Kaartnummer == kaartnummer).Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool VoertuigBestaatNiet(string chassisnummer)
        {
            if (_context.Voertuigen.Where(v => v.Chassisnummer == chassisnummer).Any())
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}