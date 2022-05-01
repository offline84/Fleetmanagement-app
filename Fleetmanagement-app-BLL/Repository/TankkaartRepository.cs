using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public class TankkaartRepository : GenericRepository<Tankkaart>, ITankkaartRepository
    {
        public TankkaartRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        public override async Task<bool> Add(Tankkaart tankkaart)
        {
            _logger.LogWarning("Tankkaart_Add: Start van Methode");
            List<ToewijzingBrandstofTankkaart> toewijzingenbrandstof = new List<ToewijzingBrandstofTankkaart>();
            tankkaart.Kaartnummer = tankkaart.Kaartnummer.Trim();

            if (VerplichteVeldenLeeg(tankkaart))
            {
                _logger.LogWarning("Tankkaart_Add: GeldigheidsDatum of Kaartnummer zijn leeg");
                return false;
            }

            if (_dbSet.Where(t => t.Kaartnummer == tankkaart.Kaartnummer).Any())
            {
                _logger.LogWarning("Tankkaart_Add: Er bestaat al een kaart met nummer " + tankkaart.Kaartnummer);
                return false;
            }

            if (tankkaart.MogelijkeBrandstoffen != null)
            {
                _logger.LogWarning("Tankkaart_Add: Start Brandstof toewijzingen");
                foreach (var brandstof in tankkaart.MogelijkeBrandstoffen)
                {
                    if (_context.Brandstof.Where(b => b.TypeBrandstof == brandstof.Brandstof.TypeBrandstof).Any())
                    {
                        tankkaart.Brandstoffen.Add(brandstof.Brandstof);
                    }
                    else
                    {
                        _logger.LogWarning("Tankkaart_Add: Branstof met Type " + brandstof.Brandstof.TypeBrandstof + "bestaat niet");
                        return false;
                    }
                }
            }

            try
            {
                tankkaart.LaatstGeupdate = DateTime.Now;
                await _dbSet.AddAsync(tankkaart);
                _logger.LogWarning("Tankkaart_Add: Tankkaart toegevoegd aan Database");
            }
            catch (Exception e)
            {
                _logger.LogError("Tankkaart_Add: Er is iets foutgelopen bij het aanmaken van tankkaart in de DB", e);
                return false;
            }
            return true;
        }

        public override async Task<bool> Delete(string kaartnummer)
        {
            _logger.LogWarning("Tankkaart_Delete: Start van Methode");

            var tankkaart = await GetById(kaartnummer);
            tankkaart.IsGearchiveerd = true;

            try
            {
                tankkaart.LaatstGeupdate = DateTime.Now;
                _dbSet.Update(tankkaart);
            }
            catch (Exception e)
            {
                _logger.LogError("Tankkaart_Delete: tankkaart niet gedelete", e);
                return false;
            }

            var koppeling = _context.Koppelingen.Where(k => k.Kaartnummer == kaartnummer).FirstOrDefault();
            if (koppeling != null)
            {
                try
                {
                    koppeling.Kaartnummer = null;
                    _context.Koppelingen.Update(koppeling);
                }
                catch (Exception e)
                {
                    _logger.LogError("Tankkaart_Delete: koppeling niet leeggemaakt", e);
                    return false;
                }
            }
            _logger.LogWarning("Tankkaart_Delete: Tankkaart van koppeling gehaald");

            _logger.LogWarning("Tankkaart_Delete: Einde van Methode");
            return true;
        }

        public override async Task<IEnumerable<Tankkaart>> GetAllActive()
        {
            return await _dbSet.Where(t => t.IsGearchiveerd == false).ToListAsync();
        }

        public override async Task<IEnumerable<Tankkaart>> GetAllArchived()
        {
            return await _dbSet.Where(t => t.IsGearchiveerd == true).ToListAsync();
        }

        public override async Task<bool> Update(Tankkaart tankkaart)
        {
            if (VerplichteVeldenLeeg(tankkaart))
            {
                _logger.LogWarning("Tankkaart_Update: GeldigheidsDatum of Kaartnummer zijn leeg");
                return false;
            }

            if (_dbSet.Where(t => t.Kaartnummer == tankkaart.Kaartnummer).Any())
            {
                _logger.LogWarning("Tankkaart_Update: Er bestaat al een kaart met nummer " + tankkaart.Kaartnummer);
                return false;
            }

            try
            {
                var tankkaartToUpdate = _context.Tankkaarten.Where(t => t.Kaartnummer == tankkaart.Kaartnummer).First();
                _dbSet.Update(tankkaartToUpdate).CurrentValues.SetValues(tankkaart);
            }
            catch (Exception e)
            {
                _logger.LogError("Tankkaart_Update: Er is iets foutgelopen bij het updaten van tankkaart {kaartnummer} in de DB", e);
                return false;
            }
            return true;
        }

        public async Task<bool> Blokkeren(string kaartnummer)
        {
            var tankkaart = await GetById(kaartnummer);
            tankkaart.IsGeblokkeerd = true;

            try
            {
                tankkaart.LaatstGeupdate = DateTime.Now;
                _dbSet.Update(tankkaart);
            }
            catch (Exception e)
            {
                _logger.LogWarning("Tankkaart_Blokeren: Er is iets foutgelopen bij updaten en blokeren van tankkaart {kaartnummer} in de DB", e);
                return false;
            }
            return true;
        }

        private bool VerplichteVeldenLeeg(Tankkaart tankkaart)
        {
            return (tankkaart.GeldigheidsDatum == default | tankkaart.Kaartnummer == "");
        }

    }
}
