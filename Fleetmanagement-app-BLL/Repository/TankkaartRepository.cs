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
            try
            {
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
                        if(!_context.Brandstof.Where(b => b.TypeBrandstof == brandstof.Brandstof.TypeBrandstof).Any())
                        {
                            _logger.LogWarning("Tankkaart_Add: Branstof met Type " + brandstof.Brandstof.TypeBrandstof + "bestaat niet");
                            return false;
                        }
                    }
                }

                tankkaart.LaatstGeupdate = DateTime.Now;
                await _dbSet.AddAsync(tankkaart);
                _logger.LogWarning("Tankkaart_Add: Tankkaart toegevoegd aan Database");

                return true;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Tankkaart_Add: " + e.Message);
                return false;
            }
        }
        
        public override async Task<bool> Delete(string kaartnummer)
        {
            _logger.LogWarning("Tankkaart_Delete: Start van Methode");

            var tankkaart = await GetById(kaartnummer);
            tankkaart.IsGearchiveerd = true;
            _logger.LogWarning("Tankkaart_Delete: IsGearchiveerd is true");

            // verwijder referentie naar koppeling.
            _logger.LogWarning("Tankkaart_Delete: Tankkaart van koppeling gehaald");

            _logger.LogWarning("Tankkaart_Delete: Einde van Methode");
            return true;
        }

        public async Task<IEnumerable<Tankkaart>> GetAllActief()
        {
            var alleTankkaarten = await GetAll();
            var activeTankkaarten = new List<Tankkaart>();
            foreach (var tankkaart in alleTankkaarten)
            {
                if (tankkaart.IsGearchiveerd == false)
                {
                    activeTankkaarten.Add(tankkaart);
                }
            }
            return activeTankkaarten;
        }

        public override async Task<bool> Update(Tankkaart tankkaart)
        {
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
            tankkaart.LaatstGeupdate = DateTime.Now;
            _dbSet.Update(tankkaart);
            return true;
        }

        public override async Task<IEnumerable<Tankkaart>> Find(Expression<Func<Tankkaart, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Blokkeren(string kaartnummer)
        {
            var tankkaart = await GetById(kaartnummer);
            tankkaart.IsGeblokkeerd = true;
            _dbSet.Update(tankkaart);
            return true;
        }

        private bool VerplichteVeldenLeeg(Tankkaart tankkaart)
        {
            return (tankkaart.GeldigheidsDatum == default | tankkaart.Kaartnummer == "");
        }

    }
}
