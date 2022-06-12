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

        /// <summary>
        /// Voegt de tankkaart en mogelijke brandstoffen toe aan de database.
        /// </summary>
        /// <remarks>
        /// - Er wordt eerst nagegaan of de verplichte velden aanwezig zijn.
        /// - De Brandstof Toewijzing wordt opgebouwd op basis van de Brandstof
        /// </remarks>
        /// <param name="tankkaart">Tankkaart object</param>
        /// <returns>boolean</returns>
        public override async Task<bool> Add(Tankkaart tankkaart)
        {
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

            if (tankkaart.MogelijkeBrandstoffen !=null)
            {
                foreach (var branstofToewijzing in tankkaart.MogelijkeBrandstoffen)
                {
                    branstofToewijzing.Tankkaartnummer = tankkaart.Kaartnummer;
                    branstofToewijzing.BrandstofId = branstofToewijzing.Brandstof.Id;
                    branstofToewijzing.Brandstof = null;
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

        /// <summary>
        /// Archiveert de tankaart (IsGearchiveerd = true).
        /// </summary>
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
                _logger.LogWarning("Tankkaart_Delete: Succes");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError("Tankkaart_Delete: tankkaart niet gedelete", e);
                return false;
            }
        }
        /// <summary>
        /// Geeft 1 Tankkaart met kaartnummer weer, samen met de koppeling en mogelijk brandstoffen
        /// </summary>
        /// <param name="kaartnummer"></param>
        /// <returns></returns>
        public override async Task<Tankkaart> GetById(string kaartnummer)
        {
            return await _dbSet.Where(t => t.Kaartnummer == kaartnummer)
                .Include(k => k.Koppeling)
                .Include(t => t.MogelijkeBrandstoffen)
                .ThenInclude(tbt => tbt.Brandstof)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        /// Geeft alle Tankkaarten weer, samen met de koppeling en mogelijk brandstoffen
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Tankkaart>> GetAll()
        {
            return await _dbSet
                .Include(k => k.Koppeling)
                .Include(t => t.MogelijkeBrandstoffen)
                .ThenInclude(tbt => tbt.Brandstof)
                .ToListAsync();
        }
        /// <summary>
        /// Geeft alle active Tankkaarten weer, samen met de koppeling en mogelijk brandstoffen
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Tankkaart>> GetAllActive()
        {
            return await _dbSet.Where(t => t.IsGearchiveerd == false)
                .Include(k => k.Koppeling)
                .Include(t => t.MogelijkeBrandstoffen)
                .ThenInclude(tbt => tbt.Brandstof)
                .ToListAsync();
        }

        /// <summary>
        /// Geeft alle verwijderde Tankkaarten weer, samen met de koppeling en mogelijk brandstoffen
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Tankkaart>> GetAllArchived()
        {
            return await _dbSet.Where(t => t.IsGearchiveerd == true)
                .Include(k => k.Koppeling)
                .Include(t => t.MogelijkeBrandstoffen)
                .ThenInclude(tbt => tbt.Brandstof)
                .ToListAsync();
        }

        /// <summary>
        /// Past de bestaande tankkaart aan met de nieuwe gegevens.
        /// </summary>
        /// <remarks>
        /// - Er wordt eerst nagegaan of de verplichte velden aanwezig zijn en of er al een tankkaart met kaartnummer bestaat
        /// - De Brandstof Toewijzing wordt opgebouwd op basis van de Brandstof
        /// </remarks>
        /// <param name="tankkaart">Tankkaart object</param>
        /// <returns>boolean</returns>
        public override Task<bool> Update(Tankkaart tankkaart)
        {
            var bestaandeToewijzingenBrandstof = _context.ToewijzingBrandstofTankkaarten.Where(tbt => tbt.Tankkaartnummer == tankkaart.Kaartnummer).ToList();

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

            if (tankkaart.MogelijkeBrandstoffen != null)
            {
                foreach (var branstofToewijzing in tankkaart.MogelijkeBrandstoffen)
                {
                    branstofToewijzing.Tankkaartnummer = tankkaart.Kaartnummer;
                    branstofToewijzing.BrandstofId = branstofToewijzing.Brandstof.Id;
                    branstofToewijzing.Brandstof = null;
                }
            }

            try
            {
                foreach (ToewijzingBrandstofTankkaart toewijzing in bestaandeToewijzingenBrandstof)
                {
                    _context.ToewijzingBrandstofTankkaarten.Remove(toewijzing);
                }
                _context.ToewijzingBrandstofTankkaarten.AddRangeAsync(tankkaart.MogelijkeBrandstoffen);

                var tankkaartToUpdate = _context.Tankkaarten.Where(t => t.Kaartnummer == tankkaart.Kaartnummer).First();
                _dbSet.Update(tankkaartToUpdate).CurrentValues.SetValues(tankkaart);
                _logger.LogWarning("Tankkaart_Update: Succes");
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                _logger.LogError("Tankkaart_Update: Er is iets foutgelopen bij het updaten van tankkaart {kaartnummer} in de DB", e);
                return Task.FromResult(false);
            }

        }

        /// <summary>
        /// Blokeert de tankkaart
        /// </summary>
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
                _logger.LogWarning("Tankkaart_Blokeren: Succes");
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
