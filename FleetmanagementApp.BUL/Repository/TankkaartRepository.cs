﻿using Fleetmanagement_app_Groep1.Database;
using FleetmanagementApp.BUL.GenericRepository;
using FleetmanagementApp.BUL.Models;
using Microsoft.Extensions.Logging;

namespace FleetmanagementApp.BUL.Repository
{
    public class TankkaartRepository : GenericRepository<TankkaartModel>, ITankkaartRepository
    {
        public TankkaartRepository(FleetmanagerContext context, ILogger logger) : base(context, logger)
        {

        }
    }
}
