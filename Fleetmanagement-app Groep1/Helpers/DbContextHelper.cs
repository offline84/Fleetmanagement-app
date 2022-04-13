using Fleetmanagement_app_DAL.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Fleetmanagement_app_Groep1.Helpers
{
    /// <summary>
    ///     De DbContexthelper creëert de DbContextOptions die nodig zijn voor de connectie met de database.
    ///     Deze helper omvat het ophalen van de connection string uit de appsettings.json file en wordt via inversion of control geinjecteerd in FleetmanagerContext.
    /// </summary>
    public static class DbContextHelper
    {
        /// <summary>
        ///     GetDbContextOptions() omvat het ophalen van de connection string uit de appsettings.json file en bouwt de DbContextOption op voor de correcte database (hier: sql).
        /// </summary>
        /// <returns>
        ///     DbContextOptions<FleetmanagerContext>
        /// </returns>

        public static DbContextOptions<FleetmanagerContext> GetDbContextOptions()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return new DbContextOptionsBuilder<FleetmanagerContext>()
                      .EnableSensitiveDataLogging()
                      .UseSqlServer(configuration.GetConnectionString("Default")).Options;
        }
    }
}