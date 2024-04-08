using LibraryManagement.Core.Interfaces.Application;
using Microsoft.Extensions.Configuration;

namespace LibraryManagement.ConsoleUI
{
    public class AppConfiguration : IAppConfiguration
    {
        private IConfiguration _configuration; //storing base interface for IConfig in a field

        public AppConfiguration() //set constructor
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json")
                .AddUserSecrets<Program>()
                .Build();
        }
        public string GetConnectionString()
        {
            return _configuration["LibraryDb"] ?? ""; //get the key
        }

        public DatabaseMode GetDatabaseMode()
        {
            var mode = _configuration.GetSection("AppSettings:Mode");

            if (mode.Value == "ORM")
            {
                return DatabaseMode.ORM;
            }

            else if (mode.Value == "DirectSQL")
            {
                return DatabaseMode.DirectSQL;
            }
            else
            {
                throw new Exception("DatabaseMode configuration key invalid.");
            }
        }
    }
}
