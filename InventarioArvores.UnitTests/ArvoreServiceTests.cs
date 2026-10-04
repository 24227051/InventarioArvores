using InventarioArvores.DTOs;
using InventarioArvores.Models;
using InventarioArvores.Services;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using NSubstitute;
using Xunit;

namespace InventarioArvores.UnitTests
{
    public class ArvoreServiceTests
    {
        private readonly IMongoDatabase _mockDatabase;
        private readonly IMongoCollection<Arvore> _mockArvoresCollection;
        private readonly IMongoCollection<Especie> _mockEspeciesCollection;
        private readonly ArvoreService _service;

        public ArvoreServiceTests()
        {
            _mockDatabase = Substitute.For<IMongoDatabase>();
            _mockArvoresCollection = Substitute.For<IMongoCollection<Arvore>>();
            _mockEspeciesCollection = Substitute.For<IMongoCollection<Especie>>();

            _mockDatabase
                .GetCollection<Arvore>("arvores", Arg.Any<MongoCollectionSettings>())
                .Returns(_mockArvoresCollection);
            _mockDatabase
                .GetCollection<Especie>("especies", Arg.Any<MongoCollectionSettings>())
                .Returns(_mockEspeciesCollection);

            _service = new ArvoreService(_mockDatabase);
        }

        [Fact]
        public async Task CreateAsync_Should_InsertArvore_When_ValidArvore()
        {
            // Arrange
            var arvore = new Arvore
            {
                Id = ObjectId.GenerateNewId().ToString(),
                EspecieId = ObjectId.GenerateNewId().ToString()
            };

            _mockArvoresCollection
                .InsertOneAsync(arvore, Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>())
                .Returns(Task.CompletedTask);

            // Act
            await _service.CreateAsync(arvore);

            // Assert
            await _mockArvoresCollection
                .Received(1)
                .InsertOneAsync(arvore, Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>());
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
            _mockArvoresCollection
                .DidNotReceive()
                .Aggregate(Arg.Any<PipelineDefinition<Arvore, BsonDocument>>(), Arg.Any<AggregateOptions>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnFalse_When_InvalidObjectId()
        {
            // Arrange
            var invalidId = "invalid-id";
            var arvore = new Arvore();

            // Act
            var result = await _service.UpdateAsync(invalidId, arvore);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_Should_ReturnTrue_When_ArvoreUpdated()
        {
            // Arrange
            var validId = ObjectId.GenerateNewId().ToString();
            var arvore = new Arvore { Id = validId };
            var replaceResult = Substitute.For<ReplaceOneResult>();

            replaceResult.MatchedCount.Returns(1);

            _mockArvoresCollection
                .ReplaceOneAsync(
                    Arg.Any<FilterDefinition<Arvore>>(),
                    Arg.Any<Arvore>(),
                    Arg.Any<ReplaceOptions>(),
                    Arg.Any<CancellationToken>())
                .Returns(replaceResult);

            // Act
            var result = await _service.UpdateAsync(validId, arvore);

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
        public async Task DeleteAsync_Should_ReturnTrue_When_ArvoreDeleted()
        {
            // Arrange
            var validId = ObjectId.GenerateNewId().ToString();
            var deleteResult = Substitute.For<DeleteResult>();

            deleteResult.DeletedCount.Returns(1);

            _mockArvoresCollection
                .DeleteOneAsync(
                    Arg.Any<FilterDefinition<Arvore>>(),
                    Arg.Any<CancellationToken>())
                .Returns(deleteResult);

            // Act
            var result = await _service.DeleteAsync(validId);

            // Assert
            result.Should().BeTrue();
        }
    }
}