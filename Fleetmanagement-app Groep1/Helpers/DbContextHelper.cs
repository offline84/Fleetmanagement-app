using Fleetmanagement_app_Groep1.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1.Helpers
{
    public static class DbContextHelper
{
    public static DbContextOptions<FleetmanagerContext> GetDbContextOptions()
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();


      return new DbContextOptionsBuilder<FleetmanagerContext>()
            .UseSqlServer(configuration.GetConnectionString("Default")).Options;

    }
}
}
