using ExpTrack.DbAccess.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.EfCore.Repositories
{
    public class ConnectionStringConfig : IConfigurationConnectionString
    {
        private readonly IConfiguration _configuration;
        private readonly ILoggerFactory _logger;

        public ConnectionStringConfig(IConfiguration configuration, ILoggerFactory logger)
        {
            this._configuration = configuration;
            this._logger = logger;
        }

        private string ConstructConnectionString(string constr)
        {
            if (string.IsNullOrEmpty(constr))
            {
                _logger.CreateLogger<ConnectionStringConfig>().LogError("Connection string key is null or empty.");
                throw new ArgumentNullException(nameof(constr), "Connection string key cannot be null or empty.");
            }
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder()
            {
                ConnectionString = constr
            };
            object? host = string.Empty;
            object? username = string.Empty;
            object? password = string.Empty;
            object? database = string.Empty;
            object? searchPath = string.Empty;
            object? port = string.Empty;
            if (!builder.TryGetValue("Host", out host) || host is null ||
                !builder.TryGetValue("Username", out username) || username is null ||
                !builder.TryGetValue("Database", out database) || database is null ||
                !builder.TryGetValue("Password", out password) || password is null ||
                !builder.TryGetValue("Port", out port) || port is null ||
                !builder.TryGetValue("SearchPath", out searchPath) || searchPath is null)
            {
                _logger.CreateLogger<ConnectionStringConfig>().LogError("One or more required connection string parameters are missing.");
                throw new ArgumentException("One or more required connection string parameters are missing.");
            }
            else return $"Host={host};Username={username};Password={password};Database={database};Port={port};SearchPath={searchPath}";
        }
        public string GetConnectionString(string constrkey)
        {
            string constr = string.Empty;

            constr = _configuration.GetSection(constrkey).Value??"";
            if (string.IsNullOrEmpty(constr))
            {
                _logger.CreateLogger<ConnectionStringConfig>().LogError($"Connection string for key '{constrkey}' is null or empty.");
                throw new ArgumentNullException(nameof(constrkey), $"Connection string for key '{constrkey}' cannot be null or empty.");
            }
            else
            {
                return ConstructConnectionString(constr);
            }
        }
    }
}