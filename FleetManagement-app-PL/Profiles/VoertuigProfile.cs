using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;

namespace FleetManagement_app_PL.Profiles
{
    public class VoertuigProfile : Profile
    {
        public VoertuigProfile()
        {
            CreateMap<Categorie, CategorieForViewingDto>();
            CreateMap<Brandstof, BrandstofForViewingDto>();
            CreateMap<Status, StatusForViewingDto>();
            CreateMap<Koppeling, KoppelingForViewingDto>();

            CreateMap<Voertuig, VoertuigForViewingDto>();
        }
    }
}