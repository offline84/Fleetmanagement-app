using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;

namespace FleetManagement_app_PL.Profiles
{
    public class BestuurderProfile : Profile
    {
        public BestuurderProfile()
        {
            CreateMap<Bestuurder, BestuurderViewingDto>();
            CreateMap<Adres, AdresViewingDto>();
            CreateMap<Rijbewijs, RijbewijsViewingDto>();

        }
    }
}
