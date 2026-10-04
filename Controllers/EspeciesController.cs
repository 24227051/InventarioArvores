using InventarioArvores.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using InventarioArvores.Services;
using Microsoft.Extensions.Logging;

namespace InventarioArvores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class EspeciesController : ControllerBase
    {
        private readonly IEspecieService _especieService;
        private readonly ILogger<EspeciesController> _logger;

        public EspeciesController(IEspecieService especieService, ILogger<EspeciesController> logger)
        {
            _especieService = especieService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Especie>>> ObterTodas()
        {
            var especies = await _especieService.GetAllAsync();
            return Ok(especies);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Especie>> ObterPorId(string id)
        {
            var especie = await _especieService.GetByIdAsync(id);
            if (especie == null)
                return BadRequest("ID inválido.");

            return Ok(especie);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Especie novaEspecie)
        {
            await _especieService.CreateAsync(novaEspecie);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaEspecie.Id }, novaEspecie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] Especie especieAtualizada)
        {
            if (!await _especieService.UpdateAsync(id, especieAtualizada)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(string id)
        {
            if (!await _especieService.DeleteAsync(id)) return NotFound();
            return NoContent();
        }
    }
}
