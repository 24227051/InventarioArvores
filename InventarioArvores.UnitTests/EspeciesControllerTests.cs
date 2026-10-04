using InventarioArvores.Controllers;
using InventarioArvores.Models;
using InventarioArvores.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace InventarioArvores.UnitTests
{
    public class EspeciesControllerTests
    {
        private readonly IEspecieService _mockService;
        private readonly ILogger<EspeciesController> _mockLogger;
        private readonly EspeciesController _controller;

        public EspeciesControllerTests()
        {
            _mockService = Substitute.For<IEspecieService>();
            _mockLogger = Substitute.For<ILogger<EspeciesController>>();
            _controller = new EspeciesController(_mockService, _mockLogger);
        }

        [Fact]
        public async Task Criar_Should_ReturnCreatedAtAction_When_ValidEspecie()
        {
            // Arrange
            var novaEspecie = new Especie
            {
                Id = "123",
                NomeCientifico = "Araucaria angustifolia",
                NomePopular = "Araucária"
            };

            _mockService
                .CreateAsync(novaEspecie)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Criar(novaEspecie);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(EspeciesController.ObterPorId));
            createdResult.RouteValues["id"].Should().Be(novaEspecie.Id);

            await _mockService.Received(1).CreateAsync(novaEspecie);
        }

        [Fact]
        public async Task ObterTodas_Should_ReturnOkResult_With_EspeciesList()
        {
            // Arrange
            var especies = new List<Especie>
            {
                new Especie
                {
                    Id = "1",
                    NomeCientifico = "Araucaria angustifolia",
                    NomePopular = "Araucária"
                },
                new Especie
                {
                    Id = "2",
                    NomeCientifico = "Tabebuia serratifolia",
                    NomePopular = "Ipê"
                }
            };

            _mockService
                .GetAllAsync()
                .Returns(especies);

            // Act
            var result = await _controller.ObterTodas();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedEspecies = okResult.Value as IEnumerable<Especie>;

            returnedEspecies.Should().HaveCount(2);
            await _mockService.Received(1).GetAllAsync();
        }

        [Fact]
        public async Task ObterPorId_Should_ReturnOkResult_When_EspecieExists()
        {
            // Arrange
            var id = "123";
            var especie = new Especie
            {
                Id = id,
                NomeCientifico = "Araucaria angustifolia",
                NomePopular = "Araucária"
            };

            _mockService
                .GetByIdAsync(id)
                .Returns(especie);

            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(especie);
        }

        [Fact]
        public async Task ObterPorId_Should_ReturnBadRequest_When_EspecieDoesNotExist()
        {
            // Arrange
            var id = "nonexistent";

            _mockService
                .GetByIdAsync(id)
                .Returns((Especie)null);

            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.Result as BadRequestObjectResult;
            badRequestResult.Value.Should().Be("ID inválido.");
        }

        [Fact]
        public async Task Atualizar_Should_ReturnNoContent_When_EspecieUpdated()
        {
            // Arrange
            var id = "123";
            var especie = new Especie
            {
                Id = id,
                NomeCientifico = "Tabebuia serratifolia",
                NomePopular = "Ipê"
            };

            _mockService
                .UpdateAsync(id, especie)
                .Returns(true);

            // Act
            var result = await _controller.Atualizar(id, especie);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            await _mockService.Received(1).UpdateAsync(id, especie);
        }

        [Fact]
        public async Task Atualizar_Should_ReturnNotFound_When_EspecieNotFound()
        {
            // Arrange
            var id = "nonexistent";
            var especie = new Especie();

            _mockService
                .UpdateAsync(id, especie)
                .Returns(false);

            // Act
            var result = await _controller.Atualizar(id, especie);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Deletar_Should_ReturnNoContent_When_EspecieDeleted()
        {
            // Arrange
            var id = "123";

            _mockService
                .DeleteAsync(id)
                .Returns(true);

            // Act
            var result = await _controller.Deletar(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            await _mockService.Received(1).DeleteAsync(id);
        }

        [Fact]
        public async Task Deletar_Should_ReturnNotFound_When_EspecieNotFound()
        {
            // Arrange
            var id = "nonexistent";

            _mockService
                .DeleteAsync(id)
                .Returns(false);

            // Act
            var result = await _controller.Deletar(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}