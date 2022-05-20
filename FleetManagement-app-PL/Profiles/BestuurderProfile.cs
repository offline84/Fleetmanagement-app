using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;
using System.Collections.Generic;

namespace FleetManagement_app_PL.Profiles
{
    public class BestuurderProfile : Profile
    {
        public BestuurderProfile()
        {
            CreateMap<Rijbewijs, RijbewijsViewingDto>().ReverseMap();
            CreateMap<Adres, AdresViewingDto>().ReverseMap();

            CreateMap<Bestuurder, BestuurderViewingDto>().ReverseMap();
        }
    }
}
