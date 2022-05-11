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

        /// <summary>Voegt de tankkaart en mogelijke brandstoffen toe aan de database.</summary>
        /// <remarks>Er wordt eerst nagegaan of de verplichte velden aanwezig zijn.</remarks>
        /// <param name="tankkaart">Tankkaart object</param>
        /// <returns>boolean</returns>
        public override async Task<bool> Add(Tankkaart tankkaart)
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

            /*foreach (var brandstof in tankkaart.MogelijkeBrandstoffen)
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
            }*/

            foreach (var branstof in tankkaart.Brandstoffen)
            {
                tankkaart.MogelijkeBrandstoffen.Add(new ToewijzingBrandstofTankkaart()
                {
                    Tankkaartnummer = tankkaart.Kaartnummer,
                    BrandstofId = branstof.Id
                });
            }

            try
            {
                //await _context.ToewijzingBrandstofTankkaarten.AddRangeAsync(toewijzingenbrandstof);
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

        /// <summary>Archiveert de tankaart (IsGearchiveerd = true).</summary>
        /// <param name="kaartnummer">kaartnummer (string) van de tankkaart</param>
        /// <returns>boolean</returns>
        public override async Task<bool> Delete(string kaartnummer)
        {
            var tankkaart = await GetById(kaartnummer);
            tankkaart.IsGearchiveerd = true;

            try
            {
                tankkaart.LaatstGeupdate = DateTime.Now;
                _dbSet.Update(tankkaart);
                _logger.LogWarning("Tankkaart_Delete: Einde van Methode");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Tankkaart_Delete: tankkaart niet gedelete", e);
                return false;
            }
        }

        public override async Task<IEnumerable<Tankkaart>> GetAll()
        {
            return await _dbSet.ToListAsync();
        }

        public override async Task<IEnumerable<Tankkaart>> GetAllActive()
        {
            return await _dbSet.Where(t => t.IsGearchiveerd == false)
                .ToListAsync();
        }

        public override async Task<IEnumerable<Tankkaart>> GetAllArchived()
        {
            return await _dbSet.Where(t => t.IsGearchiveerd == true).ToListAsync();
        }

        /// <summary>Past de bestaande tankkaart aan met de nieuwe gegevens.</summary>
        /// <remarks>Er wordt eerst nagegaan of de verplichte velden aanwezig zijn en of er al een tankkaart met kaartnummer bestaat</remarks>
        /// <param name="tankkaart">Tankkaart object</param>
        /// <returns>boolean</returns>
        public override Task<bool> Update(Tankkaart tankkaart)
        {
            if (VerplichteVeldenLeeg(tankkaart))
            {
                _logger.LogWarning("Tankkaart_Update: GeldigheidsDatum of Kaartnummer zijn leeg");
                return Task.FromResult(false);
            }

            if (!_dbSet.Where(t => t.Kaartnummer == tankkaart.Kaartnummer).Any())
            {
                _logger.LogWarning("Tankkaart_Update: Deze tankkaart bestaat niet in de DB");
                return Task.FromResult(false);
            }

            try
            {
                var tankkaartToUpdate = _context.Tankkaarten.Where(t => t.Kaartnummer == tankkaart.Kaartnummer).First();
                _dbSet.Update(tankkaartToUpdate).CurrentValues.SetValues(tankkaart);
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                _logger.LogError("Tankkaart_Update: Er is iets foutgelopen bij het updaten van tankkaart {kaartnummer} in de DB", e);
                return Task.FromResult(false);
            }

        }

        /// <summary>Blokeert de tankkaart</summary>
        /// <param name="tankkaart">Tankkaart object</param>
        /// <returns>boolean</returns>
        public async Task<bool> Blokkeren(string kaartnummer)
        {
            var tankkaart = await GetById(kaartnummer);
            tankkaart.IsGeblokkeerd = true;

            try
            {
                tankkaart.LaatstGeupdate = DateTime.Now;
                _dbSet.Update(tankkaart);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogWarning("Tankkaart_Blokeren: Er is iets foutgelopen bij updaten en blokeren van tankkaart {kaartnummer} in de DB", e);
                return false;
            }
        }

        private bool VerplichteVeldenLeeg(Tankkaart tankkaart)
        {
            return (tankkaart.GeldigheidsDatum == default | tankkaart.Kaartnummer == "");
        }

    }
}
