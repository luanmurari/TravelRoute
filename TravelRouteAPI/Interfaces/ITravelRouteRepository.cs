using TravelRouteAPI.Models;

namespace TravelRouteAPI.Interfaces
{
    public interface ITravelRouteRepository
    {
        Task Insert(TravelRoute travelRoute);
        Task<List<TravelRoute>> Get();
        Task<TravelRoute?> GetByOriginAndDestination(string origin, string destination);
        Task Delete(TravelRoute travelRoute);
        Task<bool> Update(TravelRoute travelRoute);
    }
}
