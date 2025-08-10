using ExpTrack.DbAccess.Contracts;
using ExpTrack.EfCore.Contexts;
using ExpTrack.EfCore.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.EfCore.Repositories.Adapters
{
    public class AppContextFactory<T> :IAppContextFactory<T> where T : DbContext
    {
        private IConfigurationConnectionString _configuration;
        private ILoggerFactory _loggerFactory;

        public AppContextFactory(IConfigurationConnectionString configuration, ILoggerFactory loggerFactory)
        {
            _configuration = configuration;
            _loggerFactory = loggerFactory;
        }

        protected virtual T CreateDbContext()
        {
            throw new NotImplementedException();
        }

    }
}
