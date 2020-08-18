using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace HTTAPI.Repository
{
    /// <summary>
    /// To Enable migration; IDesignTimeDbContextFactory need to be implemented
    /// </summary>
    public class ContextFactory : IDesignTimeDbContextFactory<Context>
    {
        private static IConfiguration _configuration;
        private static readonly string DotnetEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        /// <summary>
        /// CTOR of context factory
        /// </summary>
        public ContextFactory()
        {
            //   Debugger.Launch();

            Log.Logger.Debug("HTTContextFactory" + Directory.GetCurrentDirectory());
            _configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.{DotnetEnvironment}.json", false)
               .Build();
        }

        /// <summary>
        /// Create db context
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public Context CreateDbContext(string[] args)
        {
            // Debugger.Launch();
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var optionsBuilder = new DbContextOptionsBuilder<Context>();
            optionsBuilder.UseSqlServer(connectionString);
            return new Context(optionsBuilder.Options);
        }
    }
}
