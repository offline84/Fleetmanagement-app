using AutoMapper;
using Fleetmanagement_app_DAL.Entities;
using FleetManagement_app_PL.ViewModel;

namespace FleetManagement_app_PL.Automapper
{
    public class BestuurderProfile : Profile
    {
        public BestuurderProfile()
        {
            CreateMap<Bestuurder, BestuurderViewModel>();
        }
    }
}
