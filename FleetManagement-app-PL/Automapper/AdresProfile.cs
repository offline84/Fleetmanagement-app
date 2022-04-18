﻿using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;

namespace FleetManagement_app_PL.Automapper
{
    public class AdresProfile : Profile
    {
        public AdresProfile()
        {
            CreateMap<Adres, AdresViewModel>();
        }
    }
}
