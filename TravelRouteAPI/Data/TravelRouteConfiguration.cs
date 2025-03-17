using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelRouteAPI.Models;

namespace TravelRouteAPI.Data
{
    public class TravelRouteConfiguration : IEntityTypeConfiguration<TravelRoute>
    {
        public void Configure(EntityTypeBuilder<TravelRoute> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Origin).IsRequired().HasMaxLength(3);
            builder.Property(r => r.Destination).IsRequired().HasMaxLength(3);
            builder.Property(r => r.Price).IsRequired();

            builder.HasData(
                new TravelRoute { Id = 1, Origin = "GRU", Destination = "BRC", Price = 10 },
                new TravelRoute { Id = 2, Origin = "BRC", Destination = "SCL", Price = 5 },
                new TravelRoute { Id = 3, Origin = "GRU", Destination = "CDG", Price = 75 },
                new TravelRoute { Id = 4, Origin = "GRU", Destination = "SCL", Price = 20 },
                new TravelRoute { Id = 5, Origin = "GRU", Destination = "ORL", Price = 56 },
                new TravelRoute { Id = 6, Origin = "ORL", Destination = "CDG", Price = 5 },
                new TravelRoute { Id = 7, Origin = "SCL", Destination = "ORL", Price = 20 }
            );
        }
    }
}

