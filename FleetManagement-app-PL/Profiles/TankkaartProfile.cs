using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;

namespace FleetManagement_app_PL.Profiles
{
    public class TankkaartProfile : Profile
    {
        public TankkaartProfile()
        {
            CreateMap<Tankkaart, TankkaartForViewingDto>();
            CreateMap<TankkaartForViewingDto, Tankkaart>();
            CreateMap<Brandstof, BrandstofForViewingDto>();
            CreateMap<BrandstofForViewingDto, Brandstof>();
            CreateMap<ToewijzingBrandstofTankkaartForViewingDto, ToewijzingBrandstofTankkaart>();
            CreateMap<ToewijzingBrandstofTankkaart, ToewijzingBrandstofTankkaartForViewingDto>();
        }
    }
}
