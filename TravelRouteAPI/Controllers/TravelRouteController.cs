using Microsoft.AspNetCore.Mvc;
using TravelRouteAPI.Interfaces;
using TravelRouteAPI.Models;

namespace TravelRouteAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TravelRouteController : ControllerBase
    {
        private readonly ITravelRouteService _travelRouteService;

        public TravelRouteController(ITravelRouteService travelRouteService)
        {
            _travelRouteService = travelRouteService;
        }

        /// <summary>Adiciona uma nova rota de viagem.</summary>
        /// <param name="travelRouteDto">Objeto contendo origem, destino e preço.</param>
        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] TravelRouteDto travelRouteDto)
        {
            var result = await _travelRouteService.Insert(travelRouteDto);

            if (result)
            {
                return Ok("Rota cadastrada com sucesso.");
            }

            return Ok($"A rota de {travelRouteDto.Origin} para {travelRouteDto.Destination} já existe.");
        }

        /// <summary>Retorna todas as rotas cadastradas.</summary>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var routes = await _travelRouteService.Get();
            return Ok(routes);
        }

        /// <summary>Remove uma rota específica com base na origem e destino.</summary>
        /// <param name="origin">Origem da rota.</param>
        /// <param name="destination">Destino da rota.</param>
        [HttpDelete("{origin}/{destination}")]
        public async Task<IActionResult> Delete(string origin, string destination)
        {
            var success = await _travelRouteService.Delete(origin, destination);
            if (!success) return NotFound();
            return NoContent();
        }

        /// <summary>Retorna a melhor rota possível entre a origem e o destino com base no menor custo.</summary>
        /// <param name="origin">Origem da viagem.</param>
        /// <param name="destination">Destino da viagem.</param>
        [HttpGet("BestRoute/{origin}/{destination}")]
        public async Task<IActionResult> GetBest(string origin, string destination)
        {
            var bestRoute = await _travelRouteService.GetBest(origin, destination);
            if (bestRoute == null) return NotFound("Nenhuma rota encontrada");
            return Ok(bestRoute);
        }

        /// <summary>Atualiza o preço de uma rota existente.</summary>
        /// <param name="travelRouteDto">Objeto contendo origem, destino e o novo preço.</param>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] TravelRouteDto travelRouteDto)
        {
            var updated = await _travelRouteService.Update(travelRouteDto);

            if (!updated)
                return NotFound("Rota não encontrada");

            return Ok("Rota atualizada com sucesso");
        }
    }
}
