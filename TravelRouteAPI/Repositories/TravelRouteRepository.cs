using Microsoft.EntityFrameworkCore;
using TravelRouteAPI.Data;
using TravelRouteAPI.Interfaces;
using TravelRouteAPI.Models;

namespace TravelRouteAPI.Repositories
{
    public class TravelRouteRepository : ITravelRouteRepository
    {
        private readonly RepositoryContext _context;

        public TravelRouteRepository(RepositoryContext context)
        {
            _context = context;
        }

        public async Task Insert(TravelRoute travelRoute)
        {
            await _context.TravelRoutes.AddAsync(travelRoute);
            await _context.SaveChangesAsync();
        }

        public async Task<List<TravelRoute>> Get()
        {
            return await _context.TravelRoutes.ToListAsync();
        }

        public async Task<TravelRoute?> GetByOriginAndDestination(string origin, string destination)
        {
            return await _context.TravelRoutes
                .FirstOrDefaultAsync(r => r.Origin == origin && r.Destination == destination);
        }

        public async Task Delete(TravelRoute travelRoute)
        {
            _context.TravelRoutes.Remove(travelRoute);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Update(TravelRoute travelRoute)
        {
            var existingRoute = await _context.TravelRoutes
                .FirstOrDefaultAsync(r => r.Origin == travelRoute.Origin && r.Destination == travelRoute.Destination);

            if (existingRoute == null)
                return false; 

            existingRoute.Price = travelRoute.Price; 
            await _context.SaveChangesAsync(); 

            return true;
        }
    }
}

