using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using InventarioArvores.Controllers;
using InventarioArvores.DTOs;
using InventarioArvores.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using NSubstitute;
using Xunit;

namespace InventarioArvores.UnitTests
{
    // Testes do controlador ArvoresController
    //public class ArvoresControllerTests
    //{
    //    private static ArvoresController CreateControllerWithCollections(
    //        IMongoCollection<Arvore>? arvoresCollection = null,
    //        IMongoCollection<Especie>? especiesCollection = null)
    //    {
    //        var database = Substitute.For<IMongoDatabase>();
    //        var arvCol = arvoresCollection ?? Substitute.For<IMongoCollection<Arvore>>();
    //        var espCol = especiesCollection ?? Substitute.For<IMongoCollection<Especie>>();

    //        database.GetCollection<Arvore>(Arg.Any<string>()).ReturnsForAnyArgs(arvCol);
    //        database.GetCollection<Especie>(Arg.Any<string>()).ReturnsForAnyArgs(espCol);

    //        return new ArvoresController(database);
    //    }

    //    [Fact]
    //    public async Task GetAll_ReturnsOkWithList()
    //    {
    //        // Arrange: monta documento BSON que será desserializado para ArvoreDetalhadaDto
    //        var id = ObjectId.GenerateNewId().ToString();
    //        var especieId = ObjectId.GenerateNewId().ToString();

    //        var doc = new BsonDocument
    //        {
    //            { "_id", new ObjectId(id) },
    //            { "especie_id", new ObjectId(especieId) },
    //            { "data_registro", DateTime.UtcNow },
    //            { "status_viva", true },
    //            { "localizacao", new BsonDocument { { "type", "Point" }, { "coordinates", new BsonArray { 1.0, 2.0 } } } },
    //            { "dendrometria", new BsonDocument { { "dap", 10.0 }, { "altura", 5.0 } } },
    //            { "fotos", new BsonArray() },
    //            { "laudos_tecnicos", new BsonArray() },
    //            { "especie", new BsonDocument { { "_id", new ObjectId(especieId) }, { "nome_cientifico", "Specie X" }, { "nome_popular", "Popular X" } } }
    //        };

    //        var documentos = new List<BsonDocument> { doc };

    //        // Substitutos para a cadeia de agregação:
    //        var aggregateArvore = Substitute.For<IAggregateFluent<Arvore>>();
    //        var aggregateBson = Substitute.For<IAggregateFluent<BsonDocument>>();

    //        // Primeira agregação em Arvore
    //        aggregateArvore.Match(Arg.Any<FilterDefinition<Arvore>>()).ReturnsForAnyArgs(aggregateArvore);

    //        // O resultado do Match é passado, mas Lookup precisa ser mockado no aggregateBson
    //        aggregateBson.Project(Arg.Any<BsonDocument>()).ReturnsForAnyArgs(aggregateBson);
    //        aggregateBson.ToListAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult(documentos));

    //        // Não teste o Lookup especificamente ou use uma abordagem diferente
    //        var collection = Substitute.For<IMongoCollection<Arvore>>();
    //        collection.Aggregate().Returns(aggregateArvore);

    //        var controller = CreateControllerWithCollections(collection);

    //        // Act
    //        var result = await controller.ObterTodas();

    //        // Assert
    //        var ok = result.Result as OkObjectResult;
    //        ok.Should().NotBeNull();
    //        var list = ok!.Value as IEnumerable<ArvoreDetalhadaDto>;
    //        list.Should().NotBeNull();
    //        list!.Should().HaveCount(1);
    //        list.Should().ContainSingle(a => a.Id == id && a.EspecieId == especieId);
    //    }

    //    [Fact]
    //    public async Task GetById_InvalidId_ReturnsBadRequest()
    //    {
    //        var controller = CreateControllerWithCollections();

    //        var result = await controller.ObterPorId("invalid-id");

    //        result.Result.Should().BeOfType<BadRequestObjectResult>();
    //    }

    //    [Fact]
    //    public async Task GetById_NotFound_ReturnsNotFound()
    //    {
    //        var aggregateArvore = Substitute.For<IAggregateFluent<Arvore>>();
    //        var aggregateBson = Substitute.For<IAggregateFluent<BsonDocument>>();

    //        aggregateArvore.Match(Arg.Any<Expression<Func<Arvore, bool>>>()).Returns(aggregateArvore);
    //        aggregateArvore.Lookup(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(aggregateBson);
    //        aggregateBson.Project(Arg.Any<BsonDocument>()).Returns(aggregateBson);
    //        aggregateBson.FirstOrDefaultAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<BsonDocument?>(null));

    //        var collection = Substitute.For<IMongoCollection<Arvore>>();
    //        collection.Aggregate().Returns(aggregateArvore);

    //        var controller = CreateControllerWithCollections(collection);

    //        var id = ObjectId.GenerateNewId().ToString();
    //        var result = await controller.ObterPorId(id);

    //        result.Result.Should().BeOfType<NotFoundResult>();
    //    }

    //    [Fact]
    //    public async Task GetById_Found_ReturnsOkWithItem()
    //    {
    //        var id = ObjectId.GenerateNewId().ToString();
    //        var especieId = ObjectId.GenerateNewId().ToString();

    //        var doc = new BsonDocument
    //        {
    //            { "_id", new ObjectId(id) },
    //            { "especie_id", new ObjectId(especieId) },
    //            { "data_registro", DateTime.UtcNow },
    //            { "status_viva", true },
    //            { "localizacao", new BsonDocument { { "type", "Point" }, { "coordinates", new BsonArray { 1.0, 2.0 } } } },
    //            { "dendrometria", new BsonDocument { { "dap", 10.0 }, { "altura", 5.0 } } },
    //            { "fotos", new BsonArray() },
    //            { "laudos_tecnicos", new BsonArray() },
    //            { "especie", new BsonDocument { { "_id", new ObjectId(especieId) }, { "nome_cientifico", "Specie X" } } }
    //        };

    //        var aggregateArvore = Substitute.For<IAggregateFluent<Arvore>>();
    //        var aggregateBson = Substitute.For<IAggregateFluent<BsonDocument>>();

    //        aggregateArvore.Match(Arg.Any<Expression<Func<Arvore, bool>>>()).Returns(aggregateArvore);
    //        aggregateArvore.Lookup(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>()).Returns(aggregateBson);
    //        aggregateBson.Project(Arg.Any<BsonDocument>()).Returns(aggregateBson);
    //        aggregateBson.FirstOrDefaultAsync(Arg.Any<CancellationToken>()).Returns(Task.FromResult<BsonDocument?>(doc));

    //        var collection = Substitute.For<IMongoCollection<Arvore>>();
    //        collection.Aggregate().Returns(aggregateArvore);

    //        var controller = CreateControllerWithCollections(collection);

    //        var result = await controller.ObterPorId(id);

    //        var ok = result.Result as OkObjectResult;
    //        ok.Should().NotBeNull();
    //        var item = ok!.Value as ArvoreDetalhadaDto;
    //        item.Should().NotBeNull();
    //        item!.Id.Should().Be(id);
    //        item.EspecieId.Should().Be(especieId);
    //    }

    //    [Fact]
    //    public async Task Create_CallsInsertAndReturnsCreated()
    //    {
    //        var collection = Substitute.For<IMongoCollection<Arvore>>();
    //        collection.InsertOneAsync(Arg.Any<Arvore>(), Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>()).ReturnsForAnyArgs(Task.CompletedTask);

    //        var controller = CreateControllerWithCollections(collection);

    //        var newItem = new Arvore { Id = ObjectId.GenerateNewId().ToString(), EspecieId = ObjectId.GenerateNewId().ToString() };
    //        var result = await controller.Criar(newItem);

    //        var created = result as CreatedAtActionResult;
    //        created.Should().NotBeNull();
    //        created!.RouteValues!["id"].Should().Be(newItem.Id);
    //        await collection.Received(1).InsertOneAsync(newItem, Arg.Any<InsertOneOptions>(), Arg.Any<CancellationToken>());
    //    }

    //    [Fact]
    //    public async Task Update_InvalidId_ReturnsBadRequest()
    //    {
    //        var controller = CreateControllerWithCollections();

    //        var result = await controller.Atualizar("invalid-id", new Arvore());

    //        result.Should().BeOfType<BadRequestObjectResult>();
    //    }

    //    [Fact]
    //    public async Task Update_NotFound_ReturnsNotFound()
    //    {
    //        var replaceResult = Substitute.For<ReplaceOneResult>();
    //        replaceResult.MatchedCount.Returns(0);

    //        var collection = Substitute.For<IMongoCollection<Arvore>>();

    //        // 2. Mantendo os argumentos corretos para evitar a ambiguidade do NSubstitute
    //        collection.ReplaceOneAsync(
    //            Arg.Any<FilterDefinition<Arvore>>(),
    //            Arg.Any<Arvore>(),
    //            Arg.Any<ReplaceOptions>(),
    //            Arg.Any<CancellationToken>()
    //        )
    //        .Returns(Task.FromResult(replaceResult));

    //        var controller = CreateControllerWithCollections(collection);

    //        var id = ObjectId.GenerateNewId().ToString();
    //        var result = await controller.Atualizar(id, new Arvore());

    //        result.Should().BeOfType<NotFoundResult>();
    //    }

    //    [Fact]
    //    public async Task Update_Success_ReturnsNoContent()
    //    {
    //        var replaceResult = Substitute.For<ReplaceOneResult>();
    //        replaceResult.MatchedCount.Returns(1);

    //        var collection = Substitute.For<IMongoCollection<Arvore>>();
    //        collection.ReplaceOneAsync(Arg.Any<FilterDefinition<Arvore>>(), Arg.Any<Arvore>(), Arg.Any<ReplaceOptions>(), Arg.Any<CancellationToken>())
    //                  .ReturnsForAnyArgs(Task.FromResult(replaceResult));

    //        var controller = CreateControllerWithCollections(collection);

    //        var id = ObjectId.GenerateNewId().ToString();
    //        var result = await controller.Atualizar(id, new Arvore());

    //        result.Should().BeOfType<NoContentResult>();
    //        await collection.Received(1).ReplaceOneAsync(Arg.Any<FilterDefinition<Arvore>>(), Arg.Any<Arvore>(), Arg.Any<ReplaceOptions>(), Arg.Any<CancellationToken>());
    //    }

    //    [Fact]
    //    public async Task Delete_InvalidId_ReturnsBadRequest()
    //    {
    //        var controller = CreateControllerWithCollections();

    //        var result = await controller.Deletar("invalid-id");

    //        result.Should().BeOfType<BadRequestObjectResult>();
    //    }

    //    [Fact]
    //    public async Task Delete_NotFound_ReturnsNotFound()
    //    {
    //        var deleteResult = Substitute.For<DeleteResult>();
    //        deleteResult.DeletedCount.Returns(0);

    //        var collection = Substitute.For<IMongoCollection<Arvore>>();
    //        collection.DeleteOneAsync(Arg.Any<FilterDefinition<Arvore>>(), Arg.Any<CancellationToken>())
    //                  .ReturnsForAnyArgs(Task.FromResult(deleteResult));

    //        var controller = CreateControllerWithCollections(collection);

    //        var id = ObjectId.GenerateNewId().ToString();
    //        var result = await controller.Deletar(id);

    //        result.Should().BeOfType<NotFoundResult>();
    //    }

    //    [Fact]
    //    public async Task Delete_Success_ReturnsNoContent()
    //    {
    //        var deleteResult = Substitute.For<DeleteResult>();
    //        deleteResult.DeletedCount.Returns(1);

    //        var collection = Substitute.For<IMongoCollection<Arvore>>();
    //        collection.DeleteOneAsync(Arg.Any<FilterDefinition<Arvore>>(), Arg.Any<CancellationToken>())
    //                  .ReturnsForAnyArgs(Task.FromResult(deleteResult));

    //        var controller = CreateControllerWithCollections(collection);

    //        var id = ObjectId.GenerateNewId().ToString();
    //        var result = await controller.Deletar(id);

    //        result.Should().BeOfType<NoContentResult>();
    //        await collection.Received(1).DeleteOneAsync(Arg.Any<FilterDefinition<Arvore>>(), Arg.Any<CancellationToken>());
    //    }
    //}
}