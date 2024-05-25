using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Identity.API.Factories
{
    public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
               .AddJsonFile("appsettings.json")
               .AddEnvironmentVariables()
               .AddUserSecrets(Assembly.GetExecutingAssembly())
               .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            var storeOptions = new ConfigurationStoreOptions();

            optionsBuilder.UseSqlServer(config.GetConnectionString("AppDbContext"), sqlServerOptionsAction: o => o.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));


            return new ConfigurationDbContext(optionsBuilder.Options, storeOptions);
        }
    }
}