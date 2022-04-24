using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace FleetManagement_app_PL.Profiles
{
    public class VoertuigProfile: Profile
    {
        public VoertuigProfile()
        {
            CreateMap<Categorie, CategorieForViewingDto>();
            CreateMap<Brandstof, BrandstofForViewingDto>();
            CreateMap<Status, StatusForViewingDto>();

            CreateMap<Voertuig, VoertuigForViewingDto>();
        }
    }
}
