using Fleetmanagement_app_BLL.GenericRepository;
using Fleetmanagement_app_DAL.Database;
using Fleetmanagement_app_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fleetmanagement_app_BLL.Repository
{
    public class VoertuigRepository : GenericRepository<Voertuig>, IVoertuigRepository
    {
        private FleetmanagerContext _context;
        private readonly ILogger _logger;

        public VoertuigRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {
            _logger = logger;
            _context = context;
        }

        /// <summary>
        ///     Add() voegt het voertuig toe aan de database.
        ///     BulkAdds zijn niet geimplementeerd.
        /// </summary>
        /// <remarks>
        ///     Let wel: voertuig dient eerst gebouwd te worden via de buildersklasse vooraleer mee te geven aan de method.
        /// </remarks>
        /// <param name="voertuig"></param>
        /// <returns>boolean</returns>

        public override async Task<bool> Add(Voertuig voertuig)
        {
            try
            {
                if (_dbSet.Where(v => v.Chassisnummer == voertuig.Chassisnummer).Any())
                {
                    _logger.LogWarning("De database bevat reeds een voertuig met hetzelfde chassisnummer!", voertuig.Chassisnummer);
                    return false;
                }

                if (voertuig.Nummerplaat.Trim() != "")
                {
                    if (_dbSet.Where(v => v.Nummerplaat.ToUpper() == voertuig.Nummerplaat.ToUpper()).Any())
                    {
                        _logger.LogWarning("De database bevat reeds een voertuig met dezelfde nummerplaat!", voertuig.Nummerplaat);
                        return false;
                    }
                }

                voertuig.LaatstGeupdate = DateTime.Now;
                await _dbSet.AddAsync(voertuig);
                return true;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, e.Message);
                return false;
            }
        }

        /// <summary>
        ///     Archiveert het voertuig.
        /// </summary>
        /// <param string ="chassisnummer"></param>
        /// <returns> boolean </returns>

        public override async Task<bool> Delete(string chassisnummer)
        {
            var voertuig = await GetById(chassisnummer);

            if (voertuig != null)
            {
                voertuig.IsGearchiveerd = true;
                voertuig.LaatstGeupdate = DateTime.Now;

                try
                {
                    _dbSet.Update(voertuig);
                }
                catch (Exception e)
                {
                    _logger.LogError("Deleting voertuig {chassisnummer} gave error", e);
                    return false;
                }

                return true;
            }
            _logger.LogWarning("Voertuig for deletion did not exist in Database" );
            return false;
        }

        public async override Task<IEnumerable<Voertuig>> GetAll()
        {
            return await _dbSet
                //.Include(b => b.Brandstof)
                //.Include(c => c.Categorie)
                //.Include(s => s.Status)
                .ToListAsync();
        }

        public async override Task<IEnumerable<Voertuig>> GetAllArchived()
        {
            return await _dbSet.Where(v => v.IsGearchiveerd == true)
                //.Include(b => b.Brandstof)
                //.Include(c => c.Categorie)
                //.Include(s => s.Status)
                .ToListAsync();
        }
        public async override Task<IEnumerable<Voertuig>> GetAllActive()
        {
            return await _dbSet.Where(v => v.IsGearchiveerd == false)
                //.Include(b => b.Brandstof)
                //.Include(c => c.Categorie)
                //.Include(s => s.Status)
                .ToListAsync();
        }

        public override async Task<Voertuig> GetById(string chassisnummer)
        {
            return await _dbSet.Where(v => v.Chassisnummer == chassisnummer)
                .Include(b => b.Brandstof)
                .Include(c => c.Categorie)
                .Include(s => s.Status)
                .FirstOrDefaultAsync();
        }

        /// <summary>
        ///     Deze method past de gegevens van een bestaand voertuig aan.
        /// </summary>
        /// <remarks>
        ///     Eerst wordt er nagegaan of het voertuig de benodigde parameters bevat. 
        ///     Vervolgens wordt
        ///     <code>
        ///         voertuig.LaatstGeupdate = DateTime.Now;
        ///         voertuig.CategorieId = voertuig.Categorie.Id;
        ///         voertuig.BrandstofId = voertuig.Brandstof.Id;
        ///         if(voertuig.Status != null)
        ///         voertuig.StatusId = voertuig.Status.Id;
        ///     </code>
        ///     uitgevoerd om de properties te corrigeren indien nodig.
        ///     Tot slot wordt er nagegaan of het voertuig reeds bestaat in de database, indien niet zal de update niet uitgevoerd worden.
        /// </remarks>
        /// <param name="voertuig"></param>
        /// <returns>bool</returns>
        public override Task<bool> Update(Voertuig voertuig)
        {
            if (!RequiredPropertiesCheck(voertuig))
            {
                return Task.FromResult(false);
            }

            voertuig.LaatstGeupdate = DateTime.Now;
            voertuig.CategorieId = voertuig.Categorie.Id;
            voertuig.BrandstofId = voertuig.Brandstof.Id;
            if (voertuig.Status != null)
                voertuig.StatusId = voertuig.Status.Id;


            try
            {
                var voertuigToUpdate = _context.Voertuigen.Where(v => v.Chassisnummer == voertuig.Chassisnummer).First();

                _dbSet.Update(voertuigToUpdate).CurrentValues.SetValues(voertuig);

                return Task.FromResult(true);
            }
            catch (Exception e)
            {
                _logger.LogError("Something went wrong while updating voertuig {voertuig.Chassisnummer}", e);
                return Task.FromResult(false);
            }

        }

        public override async Task<IEnumerable<Voertuig>> Find(Expression<Func<Voertuig, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }


        /// <summary>
        ///     Deze Method controleert of de verplichte velden al dan niet zijn ingevuld.
        /// </summary>
        /// <param name="voertuig"></param>
        /// <returns>boolean</returns>
        public bool RequiredPropertiesCheck(Voertuig voertuig)
        {
            voertuig.Chassisnummer = voertuig.Chassisnummer.Trim().ToUpper();
            voertuig.Merk = voertuig.Merk.Trim();
            voertuig.Model = voertuig.Model.Trim();

            if (voertuig.Chassisnummer == "" | voertuig.Merk == "" | voertuig.Model == "" | voertuig.Categorie == null | voertuig.Brandstof == null)
            {
                _logger.LogWarning("one or more required fields for update are empty: voertuig.Merk, voertuig.Model, voertuig.Nummerplaat, voertuig.Categorie, voertuig.Brandstof",
                    voertuig.Merk, voertuig.Model, voertuig.Nummerplaat, voertuig.Categorie, voertuig.Brandstof);

                return false;
            }

            if (voertuig.Status == null && voertuig.Nummerplaat.Trim() == "")
            {
                _logger.LogWarning("Licenseplate cannot be Empty for update");
                return false;
            }

            if (voertuig.Status != null && voertuig.Status.Staat != "aankoop" && voertuig.Nummerplaat.Trim() == "")
            {
                _logger.LogWarning("Licenseplate cannot be Empty for update");
                return false;
            }


            return true;
        }
    }
}