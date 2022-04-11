using Fleetmanagement_app_Groep1.Database;
using Fleetmanagement_app_Groep1.Entities;
using Fleetmanagement_app_BLL.GenericRepository;
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
            List<ToewijzingBrandstofTankkaart> toewijzingenbrandstof = new List<ToewijzingBrandstofTankkaart>();
            tankkaart.Kaartnummer = tankkaart.Kaartnummer.Trim();

            if (tankkaart.GeldigheidsDatum == null | tankkaart.Kaartnummer == "")
            {
                return false;
            }

            foreach (var brandstof in tankkaart.MogelijkeBrandstoffen)
            {
                ToewijzingBrandstofTankkaart toewijzingBrandTank = new ToewijzingBrandstofTankkaart();
                toewijzingBrandTank.Tankkaart = tankkaart;
                toewijzingBrandTank.Tankkaartnummer = tankkaart.Kaartnummer;
                //To check
                toewijzingBrandTank.Brandstof = brandstof.Brandstof;
                toewijzingBrandTank.BrandstofId = brandstof.Brandstof.Id;

                toewijzingenbrandstof.Add(toewijzingBrandTank);
            }

            await _context.ToewijzingBrandstofTankkaarten.AddRangeAsync(toewijzingenbrandstof);
            await _dbSet.AddAsync(tankkaart);

            return true;
        }

        public override async Task<bool> Delete(string kaartnummer)
        {

            var tankkaart = await GetById(kaartnummer);
            tankkaart.IsGearchiveerd = true;
            foreach (var toewijzingBrandstof in tankkaart.MogelijkeBrandstoffen)
            {
                //delete Toewijzingen? Niet nodig denk ik.
            }
            // verwijder referentie naar koppeling wel nodig.

            return true;   
        }

        //overrid nodig?
        public override async Task<IEnumerable<Tankkaart>> GetAll()
        {
            var tankkaarten = await _context.Tankkaarten.ToListAsync();
            return tankkaarten;
        }

        //overrid nodig?
        public override async Task<Tankkaart> GetById(string kaartnummer)
        {
            var tankkaart = await _context.Tankkaarten.FindAsync(kaartnummer);
            return tankkaart;
        }

        public override Task<bool> Update(Tankkaart tankkaart)
        {
            throw new NotImplementedException();
        }

        public override async Task<IEnumerable<Tankkaart>> Find(Expression<Func<Tankkaart, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Blokkeren(Tankkaart tankkaart)
        {
            throw new NotImplementedException();
        }

    }

}
