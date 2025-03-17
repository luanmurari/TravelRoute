using TravelRouteAPI.Models;

namespace TravelRouteAPI.Interfaces
{
    public interface ITravelRouteService
    {
        Task<bool> Insert(TravelRouteDto travelRouteDto);
        Task<List<TravelRoute>> Get();
        Task<bool> Delete(string origin, string destination);
        Task<string> GetBest(string origin, string destination);
        Task<bool> Update(TravelRouteDto travelRouteDto);
    }
}
