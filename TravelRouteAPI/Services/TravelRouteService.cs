using TravelRouteAPI.Interfaces;
using TravelRouteAPI.Models;

namespace TravelRouteAPI.Services
{
    public class TravelRouteService : ITravelRouteService
    {
        private readonly ITravelRouteRepository _travelRouteRepository;
        private readonly ILogger<TravelRouteService> _logger;

        public TravelRouteService(ITravelRouteRepository travelRouteRepository, ILogger<TravelRouteService> logger)
        {
            _travelRouteRepository = travelRouteRepository;
            _logger = logger;
        }

        public async Task<bool> Insert(TravelRouteDto travelRouteDto)
        {
            try
            {
                var existingRoute = await _travelRouteRepository.GetByOriginAndDestination(travelRouteDto.Origin, travelRouteDto.Destination);

                if (existingRoute != null)
                {
                    return false;
                }

                var travelRoute = new TravelRoute
                {
                    Origin = travelRouteDto.Origin,
                    Destination = travelRouteDto.Destination,
                    Price = travelRouteDto.Price
                };

                await _travelRouteRepository.Insert(travelRoute);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao inserir a rota: {Origin} - {Destination}, Preço: {Price}", travelRouteDto.Origin, travelRouteDto.Destination, travelRouteDto.Price);
                throw new ApplicationException("Ocorreu um erro ao cadastrar a rota.");
            }
        }

        public async Task<List<TravelRoute>> Get()
        {
            try
            {
                return await _travelRouteRepository.Get();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar rotas.");
                throw new ApplicationException("Ocorreu um erro ao buscar as rotas.");
            }
        }

        public async Task<bool> Delete(string origin, string destination)
        {
            try
            {
                var travelRoute = await _travelRouteRepository.GetByOriginAndDestination(origin, destination);
                if (travelRoute == null) return false;

                await _travelRouteRepository.Delete(travelRoute);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao deletar a rota: {Origin} - {Destination}", origin, destination);
                throw new ApplicationException("Ocorreu um erro ao deletar a rota.");
            }
        }

        public async Task<string> GetBest(string origin, string destination)
        {
            try
            {
                var travelRoutes = await _travelRouteRepository.Get();

                var bestPath = FindCheapestRoute(travelRoutes, origin, destination);

                return bestPath ?? "Nenhuma rota encontrada";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao buscar a melhor rota de {Origin} para {Destination}", origin, destination);
                throw new ApplicationException("Ocorreu um erro ao calcular a melhor rota.");
            }
        }

        private string? FindCheapestRoute(List<TravelRoute> routes, string origin, string destination)
        {
            try
            {
                var graph = new Dictionary<string, List<(string Destination, int Price)>>();

                foreach (var route in routes)
                {
                    if (!graph.ContainsKey(route.Origin))
                        graph[route.Origin] = new List<(string, int)>();

                    graph[route.Origin].Add((route.Destination, route.Price));
                }

                if (!graph.ContainsKey(origin))
                    return null;

                var bestPrices = new Dictionary<string, int>();
                var previousNodes = new Dictionary<string, string>();
                var priorityQueue = new SortedSet<(int Price, string Node)>();

                bestPrices[origin] = 0;
                priorityQueue.Add((0, origin));

                while (priorityQueue.Count > 0)
                {
                    var (currentPrice, currentNode) = priorityQueue.Min;
                    priorityQueue.Remove(priorityQueue.Min);

                    if (currentNode == destination)
                        break;

                    if (!graph.ContainsKey(currentNode))
                        continue;

                    foreach (var (nextNode, price) in graph[currentNode])
                    {
                        int newPrice = currentPrice + price;

                        if (!bestPrices.ContainsKey(nextNode) || newPrice < bestPrices[nextNode])
                        {
                            bestPrices[nextNode] = newPrice;
                            previousNodes[nextNode] = currentNode;
                            priorityQueue.Add((newPrice, nextNode));
                        }
                    }
                }

                if (!bestPrices.ContainsKey(destination))
                    return null;

                var path = new List<string>();
                string current = destination;

                while (current != origin)
                {
                    path.Add(current);
                    current = previousNodes[current];
                }

                path.Add(origin);
                path.Reverse();

                return $"{string.Join(" - ", path)} ao custo de ${bestPrices[destination]}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no cálculo da melhor rota de {Origin} para {Destination}", origin, destination);
                throw new ApplicationException("Ocorreu um erro interno ao calcular a melhor rota.");
            }
        }

        public async Task<bool> Update(TravelRouteDto travelRouteDto)
        {
            try
            {
                var existingRoute = await _travelRouteRepository.GetByOriginAndDestination(travelRouteDto.Origin, travelRouteDto.Destination);

                if (existingRoute == null)
                    return false;

                existingRoute.Price = travelRouteDto.Price;
                return await _travelRouteRepository.Update(existingRoute);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao atualizar preço da rota: {Origin} - {Destination}", travelRouteDto.Origin, travelRouteDto.Destination);
                throw new ApplicationException("Ocorreu um erro ao atualizar o preço da rota.");
            }
        }
    }
}
