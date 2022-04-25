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
            }
            catch (Exception e)
            {
                _logger.LogError("Koppeling: Kon tankkaart niet koppelen aan bestuurder", e);
                return false;
            }

            _logger.LogWarning("Koppeling: Koppeling tankkaart succesvol");
            return true;
        }

        public async Task<bool> KoppelLosVanTankkaart(string kaartnummer)
        {
            if (KoppelingTankkaartBestaatNiet(kaartnummer) | TankkaartBestaatNiet(kaartnummer))
            {
                _logger.LogWarning("Los Koppelen: Koppeling Tankkaart of Tankkaart bestaan niet");
                return false;
            }

            var koppeling = _dbSet.Where(k => k.Kaartnummer == kaartnummer).FirstOrDefault();
            koppeling.Kaartnummer = null;

            try
            {
                _dbSet.Update(koppeling);
            }
            catch (Exception e)
            {
                _logger.LogError("Los Koppelen: Kon tankkaart niet los koppelen van de bestuurder", e);
                return false;
            }
            return true;
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
            }
            catch (Exception e)
            {
                _logger.LogError("Koppeling: Kon voertuig niet koppelen aan bestuurder", e);
                return false;
            }

            _logger.LogWarning("Koppeling: Koppeling voertuig succesvol");
            return true;
        }

        public async Task<bool> KoppelLosVanVoertuig(string chassisnummer)
        {
            if (KoppelingVoertuigBestaatNiet(chassisnummer) | VoertuigBestaatNiet(chassisnummer))
            {
                _logger.LogWarning("Los Koppelen: Koppeling of Voertuig bestaan niet");
                return false;
            }

            var koppeling = _dbSet.Where(k => k.Chassisnummer == chassisnummer).FirstOrDefault();
            koppeling.Kaartnummer = chassisnummer;

            try
            {
                _dbSet.Update(koppeling);
            }
            catch (Exception e)
            {
                _logger.LogError("Los Koppelen: Kon Voertuig niet los koppelen van de bestuurder", e);
                return false;
            }
            return true;
        }

        public async Task<Koppeling> GetByBestuurder(string bestuurderRRN)
        {
            if (KoppelingBestuurderBestaatNiet(bestuurderRRN))
            {
                //error
            }
            return await _dbSet.Where(k => k.Rijksregisternummer == bestuurderRRN).FirstOrDefaultAsync(); 
        }

        public async Task<Koppeling> GetByTankkaart(string kaartnummer)
        {
            if (TankkaartBestaatNiet(kaartnummer))
            {
                //error
            }
            return await _dbSet.Where(k => k.Kaartnummer == kaartnummer).FirstOrDefaultAsync();
        }

        public async Task<Koppeling> GetByvoertuig(string chassisnummer)
        {
            if (VoertuigBestaatNiet(chassisnummer))
            {
                //error
            }
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

        private bool TankkaartBestaatNiet(string kaartnummer)
        {
            if (_context.Tankkaarten.Where(k => k.Kaartnummer == kaartnummer).Any())
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