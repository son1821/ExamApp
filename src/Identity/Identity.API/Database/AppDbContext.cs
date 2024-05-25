using Identity.API.Database.Configuration;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Database
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
           builder.ApplyConfiguration(new AppUserEntityConfiguration());
            base.OnModelCreating(builder);
           
        }
    }
}
