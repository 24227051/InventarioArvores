using InventarioArvores.Controllers;
using InventarioArvores.DTOs;
using InventarioArvores.Models;
using InventarioArvores.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace InventarioArvores.UnitTests
{
    public class ArvoresControllerTests
    {
        private readonly IArvoreService _mockService;
        private readonly ILogger<ArvoresController> _mockLogger;
        private readonly ArvoresController _controller;

        public ArvoresControllerTests()
        {
            _mockService = Substitute.For<IArvoreService>();
            _mockLogger = Substitute.For<ILogger<ArvoresController>>();
            _controller = new ArvoresController(_mockService, _mockLogger);
        }

        [Fact]
        public async Task Criar_Should_ReturnCreatedAtAction_When_ValidArvore()
        {
            // Arrange
            var novaArvore = new Arvore { Id = "123", EspecieId = "456" };

            _mockService
                .CreateAsync(novaArvore)
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Criar(novaArvore);

            // Assert
            result.Should().BeOfType<CreatedAtActionResult>();
            var createdResult = result as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(ArvoresController.ObterPorId));
            createdResult.RouteValues["id"].Should().Be(novaArvore.Id);

            await _mockService.Received(1).CreateAsync(novaArvore);
        }

        [Fact]
        public async Task ObterTodas_Should_ReturnOkResult_With_ArvoreList()
        {
            // Arrange
            var arvores = new List<ArvoreDetalhadaDto>
            {
                new ArvoreDetalhadaDto { Id = "1", EspecieId = "e1" },
                new ArvoreDetalhadaDto { Id = "2", EspecieId = "e2" }
            };

            _mockService
                .GetAllAsync()
                .Returns(arvores);

            // Act
            var result = await _controller.ObterTodas();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            var returnedArvores = okResult.Value as IEnumerable<ArvoreDetalhadaDto>;

            returnedArvores.Should().HaveCount(2);
            await _mockService.Received(1).GetAllAsync();
        }

        [Fact]
        public async Task ObterPorId_Should_ReturnOkResult_When_ArvoreExists()
        {
            // Arrange
            var id = "123";
            var arvoreDto = new ArvoreDetalhadaDto { Id = id, EspecieId = "456" };

            _mockService
                .GetByIdAsync(id)
                .Returns(arvoreDto);

            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = result.Result as OkObjectResult;
            okResult.Value.Should().BeEquivalentTo(arvoreDto);
        }

        [Fact]
        public async Task ObterPorId_Should_ReturnNotFound_When_ArvoreDoesNotExist()
        {
            // Arrange
            var id = "nonexistent";

            _mockService
                .GetByIdAsync(id)
                .Returns((ArvoreDetalhadaDto)null);

            // Act
            var result = await _controller.ObterPorId(id);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Atualizar_Should_ReturnNoContent_When_ArvoreUpdated()
        {
            // Arrange
            var id = "123";
            var arvore = new Arvore { Id = id };

            _mockService
                .UpdateAsync(id, arvore)
                .Returns(true);

            // Act
            var result = await _controller.Atualizar(id, arvore);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            await _mockService.Received(1).UpdateAsync(id, arvore);
        }

        [Fact]
        public async Task Atualizar_Should_ReturnNotFound_When_ArvoreNotFound()
        {
            // Arrange
            var id = "nonexistent";
            var arvore = new Arvore();

            _mockService
                .UpdateAsync(id, arvore)
                .Returns(false);

            // Act
            var result = await _controller.Atualizar(id, arvore);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Deletar_Should_ReturnNoContent_When_ArvoreDeleted()
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
        public async Task Deletar_Should_ReturnNotFound_When_ArvoreNotFound()
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