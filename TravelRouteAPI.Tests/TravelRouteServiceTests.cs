using Moq;
using TravelRouteAPI.Interfaces;
using TravelRouteAPI.Models;
using TravelRouteAPI.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace TravelRouteAPI.Tests
{
    public class TravelRouteServiceTests
    {
        private readonly TravelRouteService _service;
        private readonly Mock<ITravelRouteRepository> _repositoryMock;
        private readonly Mock<ILogger<TravelRouteService>> _loggerMock;

        public TravelRouteServiceTests()
        {
            _repositoryMock = new Mock<ITravelRouteRepository>();
            _loggerMock = new Mock<ILogger<TravelRouteService>>();
            _service = new TravelRouteService(_repositoryMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetBest_ShouldReturnCheapestRoute()
        {
            // Arrange
            var routes = new List<TravelRoute>
            {
                new() { Origin = "GRU", Destination = "BRC", Price = 10 },
                new() { Origin = "BRC", Destination = "SCL", Price = 5 },
                new() { Origin = "GRU", Destination = "CDG", Price = 75 },
                new() { Origin = "GRU", Destination = "SCL", Price = 20 },
                new() { Origin = "GRU", Destination = "ORL", Price = 56 },
                new() { Origin = "ORL", Destination = "CDG", Price = 5 },
                new() { Origin = "SCL", Destination = "ORL", Price = 20 }
            };

            _repositoryMock.Setup(r => r.Get()).ReturnsAsync(routes);

            // Act
            var result = await _service.GetBest("GRU", "CDG");

            // Assert
            result.Should().Be("GRU - BRC - SCL - ORL - CDG ao custo de $40");
        }

        [Fact]
        public async Task GetBest_ShouldReturnNotFoundForInvalidRoute()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Get()).ReturnsAsync(new List<TravelRoute>());

            // Act
            var result = await _service.GetBest("XYZ", "ABC");

            // Assert
            result.Should().Be("Nenhuma rota encontrada");
        }

        [Fact]
        public async Task Insert_ShouldCallRepositoryOnce()
        {// Arrange
            var travelRouteDto = new TravelRouteDto { Origin = "GRU", Destination = "BRC", Price = 10 };

            // Act
            await _service.Insert(travelRouteDto);

            // Assert
            _repositoryMock.Verify(r => r.Insert(It.IsAny<TravelRoute>()), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnTrue_WhenRouteExists()
        {
            // Arrange
            var travelRoute = new TravelRoute { Origin = "GRU", Destination = "BRC", Price = 10 };
            _repositoryMock.Setup(r => r.GetByOriginAndDestination("GRU", "BRC")).ReturnsAsync(travelRoute);

            // Act
            var result = await _service.Delete("GRU", "BRC");

            // Assert
            result.Should().BeTrue();
            _repositoryMock.Verify(r => r.Delete(travelRoute), Times.Once);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenRouteDoesNotExist()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByOriginAndDestination("GRU", "BRC")).ReturnsAsync((TravelRoute)null);

            // Act
            var result = await _service.Delete("GRU", "BRC");

            // Assert
            result.Should().BeFalse();
            _repositoryMock.Verify(r => r.Delete(It.IsAny<TravelRoute>()), Times.Never);
        }

        [Fact]
        public async Task Insert_ShouldNotDuplicateExistingRoute()
        {
            // Arrange
            var travelRouteDto = new TravelRouteDto { Origin = "GRU", Destination = "BRC", Price = 10 };
            var travelRoute = new TravelRoute { Origin = "GRU", Destination = "BRC", Price = 10 };

            _repositoryMock.Setup(r => r.GetByOriginAndDestination("GRU", "BRC")).ReturnsAsync(travelRoute);

            // Act
            await _service.Insert(travelRouteDto);

            // Assert
            _repositoryMock.Verify(r => r.Insert(It.IsAny<TravelRoute>()), Times.Never);
        }

        [Fact]
        public async Task Delete_ShouldReturnFalse_WhenDatabaseIsEmpty()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByOriginAndDestination(It.IsAny<string>(), It.IsAny<string>()))
                           .ReturnsAsync((TravelRoute)null);

            // Act
            var result = await _service.Delete("GRU", "BRC");

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetBest_ShouldReturnDirectRoute_WhenAvailable()
        {
            // Arrange
            var routes = new List<TravelRoute>
            {
                new() { Origin = "GRU", Destination = "CDG", Price = 50 }
            };

            _repositoryMock.Setup(r => r.Get()).ReturnsAsync(routes);

            // Act
            var result = await _service.GetBest("GRU", "CDG");

            // Assert
            result.Should().Be("GRU - CDG ao custo de $50");
        }

        [Fact]
        public async Task GetBest_ShouldReturnAnyValidCheapestRoute_WhenMultiplePathsHaveSameCost()
        {
            // Arrange
            var routes = new List<TravelRoute>
            {
                new() { Origin = "GRU", Destination = "BRC", Price = 10 },
                new() { Origin = "BRC", Destination = "CDG", Price = 30 },
                new() { Origin = "GRU", Destination = "SCL", Price = 10 },
                new() { Origin = "SCL", Destination = "CDG", Price = 30 }
            };

            _repositoryMock.Setup(r => r.Get()).ReturnsAsync(routes);

            // Act
            var result = await _service.GetBest("GRU", "CDG");

            // Assert
            result.Should().MatchRegex(@"GRU - (BRC|SCL) - CDG ao custo de \$40");
        }

        [Fact]
        public async Task Update_ShouldUpdatePrice_WhenRouteExists()
        {
            // Arrange
            var travelRouteDto = new TravelRouteDto { Origin = "GRU", Destination = "BRC", Price = 15 };
            var existingRoute = new TravelRoute { Origin = "GRU", Destination = "BRC", Price = 10 };

            _repositoryMock.Setup(r => r.GetByOriginAndDestination("GRU", "BRC")).ReturnsAsync(existingRoute);
            _repositoryMock.Setup(r => r.Update(It.IsAny<TravelRoute>())).ReturnsAsync(true);

            // Act
            var result = await _service.Update(travelRouteDto);

            // Assert
            result.Should().BeTrue();
            existingRoute.Price.Should().Be(15);
        }

        [Fact]
        public async Task Update_ShouldReturnFalse_WhenRouteDoesNotExist()
        {
            // Arrange
            var travelRouteDto = new TravelRouteDto { Origin = "GRU", Destination = "BRC", Price = 15 };

            _repositoryMock.Setup(r => r.GetByOriginAndDestination("GRU", "BRC")).ReturnsAsync((TravelRoute)null);

            // Act
            var result = await _service.Update(travelRouteDto);

            // Assert
            result.Should().BeFalse();
        }
    }
}
