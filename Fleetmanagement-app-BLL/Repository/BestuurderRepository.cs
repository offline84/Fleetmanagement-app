using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public class BestuurderRepository : GenericRepository<Bestuurder>, IBestuurderRepository
    {
        public BestuurderRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _context = context;
        }

        /// <summary>
        /// Voegt een bestuurder toe aan de bestuurder database.
        /// </summary>
        /// <param name="bestuurder"></param>
        /// <returns></returns>
        public override async Task<bool> Add(Bestuurder bestuurder)
        {
            _logger.LogWarning("Start BestuurderRepository - AddFunction:", bestuurder);
            bestuurder.Naam = bestuurder.Naam.Trim();
            bestuurder.Achternaam = bestuurder.Achternaam.Trim();
            bestuurder.Rijksregisternummer = bestuurder.Rijksregisternummer.Trim();

            if (string.IsNullOrEmpty(bestuurder.Rijksregisternummer) |
                string.IsNullOrEmpty(bestuurder.Naam) |
                string.IsNullOrEmpty(bestuurder.Achternaam) |
                (bestuurder.GeboorteDatum == null && bestuurder.GeboorteDatum <= DateTime.MinValue)
                )
            {
                _logger.LogWarning("Something went wrong, Required values cannot be null", bestuurder);
                return false;
            }

            SetToewijzingRijbewijs(bestuurder);

            try
            {
                bestuurder.LaatstGeupdate = DateTime.Now;
                await _dbSet.AddAsync(bestuurder);
                _logger.LogWarning("End BestuurderRepository - AddFunction!");
                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong while adding bestuurder: {bestuurder.Naam} - {bestuurder.Achternaam}", e.Message);
                return false;
            }
        }

        /// <summary>
        /// Laat toe om een bestuurder te wijzigen.
        /// </summary>
        /// <param name="bestuurder"></param>
        /// <returns></returns>
        public override Task<bool> Update(Bestuurder bestuurder)
        {
            _logger.LogWarning("Start BestuurderRepository - UpdateFunction:", bestuurder);
            if (string.IsNullOrEmpty(bestuurder.Rijksregisternummer) |
              string.IsNullOrEmpty(bestuurder.Naam) |
              string.IsNullOrEmpty(bestuurder.Achternaam) |
              (bestuurder.GeboorteDatum == null && bestuurder.GeboorteDatum <= DateTime.MinValue) |
              bestuurder.ToewijzingenRijbewijs.Count == 0)
            {
                _logger.LogWarning("Something went wrong, Required values cannot be null", bestuurder);
                return Task.FromResult(false);
            }

            var entity = _dbSet.Where(b => b.Rijksregisternummer.Equals(bestuurder.Rijksregisternummer)).SingleOrDefault();

            if (entity == null)
            {
                _logger.LogWarning("Something went wrong, Bestuurder not found!", bestuurder);
                return Task.FromResult(false);
            }

            SetToewijzingRijbewijs(bestuurder);

            
            var bestuurderEntry = _context.Attach(bestuurder);
            var adressEntry = bestuurderEntry.Reference(e => e.Adres);
            adressEntry.TargetEntry.Property(a => a.Straat).IsModified = true;
            adressEntry.TargetEntry.Property(a => a.Huisnummer).IsModified = true;
            adressEntry.TargetEntry.Property(a => a.Postcode).IsModified = true;
            adressEntry.TargetEntry.Property(a => a.Stad).IsModified = true;

            try
            {
                bestuurder.LaatstGeupdate = DateTime.Now;
               
                _dbSet.Update(entity).CurrentValues.SetValues(bestuurderEntry);
                _logger.LogWarning("End BestuurderRepository - UpdateFunction!");
                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong while updating bestuurder: {bestuurder.Naam} - {bestuurder.Achternaam}", e.Message);
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// Laat toe om een bestuurder met behulp van zijn rijksregisternummer te verwijderen op een soft manier door die dan te archiveren.
        /// </summary>
        /// <param name="rijksregisternummer"></param>
        /// <returns></returns>
        public override async Task<bool> Delete(string rijksregisternummer)
        {
            var bestuurder = await GetById(rijksregisternummer);
            bestuurder.IsGearchiveerd = true;
            try
            {
                _dbSet.Update(bestuurder);
            }
            catch (Exception e)
            {
                _logger.LogError($"Something went wrong while softDeleting bestuurder: {bestuurder.Naam} - {bestuurder.Achternaam}", e.Message);
                return false;
            }
            return true;
        }

        #region Getters

        /// <summary>
        /// Geeft alle bestuurders terug.
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Bestuurder>> GetAll()
        {
            return await _dbSet
                .Include(k => k.Koppeling)
                .Include(t => t.ToewijzingenRijbewijs)
                .ThenInclude(tr => tr.Rijbewijs)
                .ToListAsync();
        }

        /// <summary>
        /// Geeft alle active bestuurders terug.
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Bestuurder>> GetAllActive()
        {
            return await _dbSet
                .Where(b => !b.IsGearchiveerd)
                .Include(k => k.Koppeling)
                .Include(t => t.ToewijzingenRijbewijs)
                .ThenInclude(tr => tr.Rijbewijs)
                .ToListAsync();
        }

        /// <summary>
        /// Geeft alle gearchiveerde bestuurderds terug.
        /// </summary>
        /// <returns></returns>
        public override async Task<IEnumerable<Bestuurder>> GetAllArchived()
        {
            return await _dbSet
                .Where(b => b.IsGearchiveerd)
                .Include(k => k.Koppeling)
                .Include(t => t.ToewijzingenRijbewijs)
                .ThenInclude(tr => tr.Rijbewijs)
                .ToListAsync();
        }

        /// <summary>
        /// Geeft een bestuurder terug afhankelijk van zijn id (rijksregisternummer).
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override async Task<Bestuurder> GetById(string id)
        {
            return await _dbSet
                .Where(b => b.Rijksregisternummer.Equals(id))
                .Include(k => k.Koppeling)
                .Include(t => t.ToewijzingenRijbewijs)
                .ThenInclude(tr => tr.Rijbewijs)
                .FirstOrDefaultAsync();
        }

        #endregion

        private static void SetToewijzingRijbewijs(Bestuurder bestuurder)
        {
            if (bestuurder.ToewijzingenRijbewijs.Count > 0)
            {
                foreach (var toewijzing in bestuurder.ToewijzingenRijbewijs)
                {
                    toewijzing.RijbewijsId = toewijzing.Rijbewijs.Id;
                    toewijzing.Rijksregisternummer = string.IsNullOrEmpty(toewijzing.Rijksregisternummer) ? bestuurder.Rijksregisternummer : toewijzing.Rijksregisternummer;
                    toewijzing.Rijbewijs = null;
                }
            }
        }
    }
}