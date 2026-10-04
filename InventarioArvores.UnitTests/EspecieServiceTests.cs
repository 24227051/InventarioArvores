using InventarioArvores.Models;
using InventarioArvores.Services;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace InventarioArvores.UnitTests
{
    public class EspecieServiceTests
    {
        private readonly IMongoDatabase _mockDatabase;
        private readonly IMongoCollection<Especie> _mockEspeciesCollection;
        private readonly EspecieService _service;

        public EspecieServiceTests()
        {
            _mockDatabase = Substitute.For<IMongoDatabase>();
            _mockEspeciesCollection = Substitute.For<IMongoCollection<Especie>>();

            _mockDatabase
                .GetCollection<Especie>("especies", Arg.Any<MongoCollectionSettings>())
                .Returns(_mockEspeciesCollection);

            _service = new EspecieService(_mockDatabase);
        }

        [Fact]
        public async Task CreateAsync_Should_InsertEspecie_When_ValidEspecie()
        {
            // Arrange
            var especie = new Especie
            {
                Id = ObjectId.GenerateNewId().ToString(),
                NomeCientifico = "Araucaria angustifolia",
                NomePopular = "Araucária"
            };

            _mockEspeciesCollection
                .InsertOneAsync(especie, Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(especie);

            // Assert
            await _mockEspeciesCollection
                .Received(1)
                .InsertOneAsync(especie, Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task GetByIdAsync_Should_ReturnNull_When_InvalidObjectId()
        {
            // Arrange
            var invalidId = "invalid-id";

            // Act
            var result = await _service.GetByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnFalse_When_InvalidObjectId()
        {
            // Arrange
            var invalidId = "invalid-id";
            var especie = new Especie();

            // Act
            var result = await _service.UpdateAsync(invalidId, especie);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnTrue_When_EspecieUpdated()
        {
            // Arrange
            var validId = ObjectId.GenerateNewId().ToString();
            var especie = new Especie
            {
                Id = validId,
                NomeCientifico = "Tabebuia serratifolia",
                NomePopular = "Ipê"
            };
            var replaceResult = Substitute.For<ReplaceOneResult>();

            replaceResult.MatchedCount.Returns(1);

            _mockEspeciesCollection
                .ReplaceOneAsync(
                    Arg.Any<FilterDefinition<Especie>>(),
                    Arg.Any<Especie>(),
                    Arg.Any<ReplaceOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(replaceResult);

            // Act
            var result = await _service.UpdateAsync(validId, especie);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteAsync_Should_ReturnFalse_When_InvalidObjectId()
        {
            // Arrange
            var invalidId = "invalid-id";

            // Act
            var result = await _service.DeleteAsync(invalidId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task DeleteAsync_Should_ReturnTrue_When_EspecieDeleted()
        {
            // Arrange
            var validId = ObjectId.GenerateNewId().ToString();
            var deleteResult = Substitute.For<DeleteResult>();

            deleteResult.DeletedCount.Returns(1);

            _mockEspeciesCollection
                .DeleteOneAsync(
                    Arg.Any<FilterDefinition<Especie>>(),
                    Arg.Any<CancellationToken>())
                .Returns(deleteResult);

            // Act
            var result = await _service.DeleteAsync(validId);

            // Assert
            result.Should().BeTrue();
        }
    }
}