using Microsoft.EntityFrameworkCore;
using TravelRouteAPI.Models;

namespace TravelRouteAPI.Data
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions<RepositoryContext> options) : base(options) { }

        public DbSet<TravelRoute> TravelRoutes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new TravelRouteConfiguration());
        }
    }
}

