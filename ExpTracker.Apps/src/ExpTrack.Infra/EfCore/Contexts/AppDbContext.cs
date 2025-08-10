using ExpTrack.DbAccess.Entities;
using ExpTrack.EfCore.Configurations;
using ExpTrack.EfCore.Configurations.EntityConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpTrack.EfCore.Contexts
{
    public class AppDbContext:DbContext
    {
        private readonly string _constr;
        private readonly ILoggerFactory _loggerFactory;

        public DbSet<Expense> Expenses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public AppDbContext(string connectionstring,ILoggerFactory loggerFactory)
        {
            this._constr = connectionstring;
            this._loggerFactory = loggerFactory;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLoggerFactory(_loggerFactory);

            var builder= optionsBuilder.UseNpgsql(_constr, npgsqlOptions =>
            {
                npgsqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromMilliseconds(1000),
                    errorCodesToAdd: Enumerable.Empty<string>().ToArray());
            });

            base.OnConfiguring(optionsBuilder);

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder != null)
            {
                modelBuilder.ApplyConfiguration(new ExpenseConfig());
                modelBuilder.ApplyConfiguration(new CategoryConfig());
                modelBuilder.ApplyConfiguration(new UserConfig());
                modelBuilder.ApplyConfiguration(new RefreshTokenConfig());
            }
        }
    }
}
