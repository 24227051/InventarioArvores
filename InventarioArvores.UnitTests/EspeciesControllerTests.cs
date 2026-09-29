using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InventarioArvores.Controllers;
using InventarioArvores.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NSubstitute;
using Xunit;

namespace InventarioArvores.UnitTests
{
    // Testes do controlador EspeciesController
    public class EspeciesControllerTests
    {
        private static EspeciesController CreateControllerWithCollection(
            IMongoCollection<Especie>? collection = null)
        {
            var database = Substitute.For<IMongoDatabase>();
            var col = collection ?? Substitute.For<IMongoCollection<Especie>>();
            // Configura retorno para GetCollection com ReturnsForAnyArgs para cobrir ambas as assinaturas
            database.GetCollection<Especie>(Arg.Any<string>()).ReturnsForAnyArgs(col);
            return new EspeciesController(database);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithList()
        {
            // Arrange: prepara lista esperada
            var expected = new List<Especie> { new Especie { Id = ObjectId.GenerateNewId().ToString(), NomeCientifico = "A" } };

            var findFluent = Substitute.For<IFindFluent<Especie, Especie>>();
            findFluent.ToListAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(expected));
            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.Find(Arg.Any<Expression<Func<Especie, bool>>>()).ReturnsForAnyArgs(findFluent);

            var controller = CreateControllerWithCollection(collection);

            // Act
            var result = await controller.ObterTodas();

            // Assert
            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            ok!.StatusCode.Should().Be(200);
            (ok.Value as IEnumerable<Especie>).Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsBadRequest()
        {
            var controller = CreateControllerWithCollection();

            var result = await controller.ObterPorId("invalid-id");

            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetById_NotFound_ReturnsNotFound()
        {
            var findFluent = Substitute.For<IFindFluent<Especie, Especie>>();
            findFluent.FirstOrDefaultAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<Especie?>(null));
            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.Find(Arg.Any<Expression<Func<Especie, bool>>>()).ReturnsForAnyArgs(findFluent);

            var controller = CreateControllerWithCollection(collection);

            var id = ObjectId.GenerateNewId().ToString();
            var result = await controller.ObterPorId(id);

            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_Found_ReturnsOkWithItem()
        {
            var item = new Especie { Id = ObjectId.GenerateNewId().ToString(), NomeCientifico = "Found" };

            var findFluent = Substitute.For<IFindFluent<Especie, Especie>>();
            findFluent.FirstOrDefaultAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<Especie?>(item));
            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.Find(Arg.Any<Expression<Func<Especie, bool>>>()).ReturnsForAnyArgs(findFluent);

            var controller = CreateControllerWithCollection(collection);

            var result = await controller.ObterPorId(item.Id);

            var ok = result.Result as OkObjectResult;
            ok.Should().NotBeNull();
            (ok!.Value as Especie).Should().BeEquivalentTo(item);
        }

        [Fact]
        public async Task Create_CallsInsertAndReturnsCreated()
        {
            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.InsertOneAsync(Arg.Any<Especie>(), Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(Task.CompletedTask);

            var controller = CreateControllerWithCollection(collection);

            var newItem = new Especie { Id = ObjectId.GenerateNewId().ToString(), NomeCientifico = "New" };
            var result = await controller.Criar(newItem);

            var created = result as CreatedAtActionResult;
            created.Should().NotBeNull();
            created!.RouteValues!["id"].Should().Be(newItem.Id);
            await collection.Received(1).InsertOneAsync(newItem, Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Update_InvalidId_ReturnsBadRequest()
        {
            var controller = CreateControllerWithCollection();

            var result = await controller.Atualizar("invalid-id", new Especie());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_NotFound_ReturnsNotFound()
        {
            // Substitui ReplaceOneResult por um substitute e define MatchedCount = 0
            var replaceResult = Substitute.For<ReplaceOneResult>();
            replaceResult.MatchedCount.Returns(0);

            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.ReplaceOneAsync(Arg.Any<Expression<Func<Especie, bool>>>(), Arg.Any<Especie>(), Arg.Any<ReplaceOptions>(), Arg.Any<CancellationToken>())
                      .ReturnsForAnyArgs(Task.FromResult(replaceResult));

            var controller = CreateControllerWithCollection(collection);

            var id = ObjectId.GenerateNewId().ToString();
            var result = await controller.Atualizar(id, new Especie());

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Update_Success_ReturnsNoContent()
        {
            // Substitui ReplaceOneResult por um substitute e define MatchedCount = 1
            var replaceResult = Substitute.For<ReplaceOneResult>();
            replaceResult.MatchedCount.Returns(1);

            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.ReplaceOneAsync(Arg.Any<Expression<Func<Especie, bool>>>(), Arg.Any<Especie>(), Arg.Any<ReplaceOptions>(), Arg.Any<CancellationToken>())
                      .ReturnsForAnyArgs(Task.FromResult(replaceResult));

            var controller = CreateControllerWithCollection(collection);

            var id = ObjectId.GenerateNewId().ToString();
            var result = await controller.Atualizar(id, new Especie());

            result.Should().BeOfType<NoContentResult>();
            await collection.Received(1).ReplaceOneAsync(Arg.Any<Expression<Func<Especie, bool>>>(), Arg.Any<Especie>(), Arg.Any<ReplaceOptions>(), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Delete_InvalidId_ReturnsBadRequest()
        {
            var controller = CreateControllerWithCollection();

            var result = await controller.Deletar("invalid-id");

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Delete_NotFound_ReturnsNotFound()
        {
            // Usa substitute para DeleteResult e define DeletedCount = 0
            var deleteResult = Substitute.For<DeleteResult>();
            deleteResult.DeletedCount.Returns(0);

            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.DeleteOneAsync(Arg.Any<Expression<Func<Especie, bool>>>(), Arg.Any<CancellationToken>())
                      .ReturnsForAnyArgs(Task.FromResult(deleteResult));

            var controller = CreateControllerWithCollection(collection);

            var id = ObjectId.GenerateNewId().ToString();
            var result = await controller.Deletar(id);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Delete_Success_ReturnsNoContent()
        {
            // Usa substitute para DeleteResult e define DeletedCount = 1
            var deleteResult = Substitute.For<DeleteResult>();
            deleteResult.DeletedCount.Returns(1);

            var collection = Substitute.For<IMongoCollection<Especie>>();
            collection.DeleteOneAsync(Arg.Any<Expression<Func<Especie, bool>>>(), Arg.Any<CancellationToken>())
                      .ReturnsForAnyArgs(Task.FromResult(deleteResult));

            var controller = CreateControllerWithCollection(collection);

            var id = ObjectId.GenerateNewId().ToString();
            var result = await controller.Deletar(id);

            result.Should().BeOfType<NoContentResult>();
            await collection.Received(1).DeleteOneAsync(Arg.Any<Expression<Func<Especie, bool>>>(), Arg.Any<CancellationToken>());
        }
    }
}