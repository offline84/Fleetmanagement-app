using Fleetmanagement_app_Groep1.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fleetmanagement_app_Groep1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            //migratie en aanmaken van de database indien deze nog niet bestaat
            using (var scope = host.Services.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<FleetmanagerContext>();
                    //check of de connectie kan gemaakt worden met de databank
                    if(!context.Database.CanConnect())
                        //Maak de databank
                        context.Database.Migrate();
                }
                catch (Exception e)
                {
                    var logbook = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                }
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
