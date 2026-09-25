using InventarioArvores.DTOs;
using InventarioArvores.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace InventarioArvores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ArvoresController : ControllerBase
    {
        private readonly IMongoCollection<Arvore> _arvoresCollection;
        private readonly IMongoCollection<Especie> _especiesCollection;

        public ArvoresController(IMongoDatabase database)
        {
            _arvoresCollection = database.GetCollection<Arvore>("arvores");
            _especiesCollection = database.GetCollection<Especie>("especies"); // Apenas para o Lookup
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Arvore novaArvore)
        {
            await _arvoresCollection.InsertOneAsync(novaArvore);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaArvore.Id }, novaArvore);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArvoreDetalhadaDto>>> ObterTodas()
        {
            var documentos = await _arvoresCollection
                .Aggregate()
                .Lookup("especies", "especie_id", "_id", "especie")
                .Project(new BsonDocument
                {
                    { "_id", 1 },
                    { "especie_id", 1 },
                    { "data_registro", 1 },
                    { "status_viva", 1 },
                    { "localizacao", 1 },
                    { "dendrometria", 1 },
                    { "fotos", 1 },
                    { "laudos_tecnicos", 1 },
                    { "especie", new BsonDocument("$arrayElemAt", new BsonArray { "$especie", 0 }) }
                })
                .ToListAsync();

            var arvores = documentos
                .Select(doc => BsonSerializer.Deserialize<ArvoreDetalhadaDto>(doc))
                .ToList();

            return Ok(arvores);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArvoreDetalhadaDto>> ObterPorId(string id)
        {
            if (!ObjectId.TryParse(id, out _))
                return BadRequest("ID inválido.");

            var documento = await _arvoresCollection
                .Aggregate()
                .Match(a => a.Id == id)
                .Lookup("especies", "especie_id", "_id", "especie")
                .Project(new BsonDocument
                {
                    { "_id", 1 },
                    { "especie_id", 1 },
                    { "data_registro", 1 },
                    { "status_viva", 1 },
                    { "localizacao", 1 },
                    { "dendrometria", 1 },
                    { "fotos", 1 },
                    { "laudos_tecnicos", 1 },
                    { "especie", new BsonDocument("$arrayElemAt", new BsonArray { "$especie", 0 }) }
                })
                .FirstOrDefaultAsync();

            if (documento == null)
                return NotFound();

            return Ok(BsonSerializer.Deserialize<ArvoreDetalhadaDto>(documento));
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] Arvore arvoreAtualizada)
        {
            if (!ObjectId.TryParse(id, out _)) return BadRequest("ID inválido.");

            arvoreAtualizada.Id = id; // Garante que o ID do corpo é o mesmo da rota
            var resultado = await _arvoresCollection.ReplaceOneAsync(e => e.Id == id, arvoreAtualizada);

            if (resultado.MatchedCount == 0) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return BadRequest("ID inválido.");

            var resultado = await _arvoresCollection.DeleteOneAsync(e => e.Id == id);
            if (resultado.DeletedCount == 0) return NotFound();

            return NoContent();
        }
    }
}
