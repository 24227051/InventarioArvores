using InventarioArvores.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InventarioArvores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EspeciesController : ControllerBase
    {
        private readonly IMongoCollection<Especie> _especiesCollection;

        public EspeciesController(IMongoDatabase database)
        {
            _especiesCollection = database.GetCollection<Especie>("especies");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especie>>> ObterTodas()
        {
            var especies = await _especiesCollection.Find(_ => true).ToListAsync();
            return Ok(especies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Especie>> ObterPorId(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return BadRequest("ID inválido.");

            var especie = await _especiesCollection.Find(e => e.Id == id).FirstOrDefaultAsync();
            if (especie == null) return NotFound();

            return Ok(especie);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Especie novaEspecie)
        {
            await _especiesCollection.InsertOneAsync(novaEspecie);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaEspecie.Id }, novaEspecie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] Especie especieAtualizada)
        {
            if (!ObjectId.TryParse(id, out _)) return BadRequest("ID inválido.");

            especieAtualizada.Id = id; // Garante que o ID do corpo é o mesmo da rota
            var resultado = await _especiesCollection.ReplaceOneAsync(e => e.Id == id, especieAtualizada);

            if (resultado.MatchedCount == 0) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(string id)
        {
            if (!ObjectId.TryParse(id, out _)) return BadRequest("ID inválido.");

            var resultado = await _especiesCollection.DeleteOneAsync(e => e.Id == id);
            if (resultado.DeletedCount == 0) return NotFound();

            return NoContent();
        }
    }
}
