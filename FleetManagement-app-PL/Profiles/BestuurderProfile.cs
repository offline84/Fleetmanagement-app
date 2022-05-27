using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;

namespace FleetManagement_app_PL.Profiles
{
    public class BestuurderProfile : Profile
    {
        public BestuurderProfile()
        {
            CreateMap<ToewijzingRijbewijsBestuurder, ToewijzingRijbewijsBestuurderViewingDto>().ReverseMap();
            CreateMap<Rijbewijs, RijbewijsViewingDto>().ReverseMap();
            CreateMap<Adres, AdresViewingDto>().ReverseMap();
            CreateMap<Koppeling, KoppelingForViewingDto>();

            CreateMap<Bestuurder, BestuurderViewingDto>().ReverseMap();
        }
    }
}
