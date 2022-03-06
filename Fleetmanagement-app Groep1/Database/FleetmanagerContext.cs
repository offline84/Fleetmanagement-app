using Microsoft.Extensions.Configuration;
using System.Data.Entity;

namespace Fleetmanagement_app_Groep1.Database
{
    [DbConfigurationType(typeof(FleetManagerDbConfig))]
    public class FleetmanagerContext : DbContext
    {
        private string _connectionstring;

        public string Connectionstring
        {
            get => _connectionstring;
            set
            {
                var config = new ConfigurationBuilder()
                   .AddJsonFile("dbsettings.json", optional: true)
                   .AddEnvironmentVariables()
                   .Build();

                string datasource = config.GetValue<string>("DataSource");
                string catalog = config.GetValue<string>("Catalog");

                _connectionstring = $"@Data Source={datasource}; Initial Catalog={catalog};Integrated Security=true";
            }
        }

        public FleetmanagerContext(string connection = Connectionstring): base(connection);
        {

        }
    }
}