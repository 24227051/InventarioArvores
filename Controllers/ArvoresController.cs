using InventarioArvores.DTOs;
using InventarioArvores.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using InventarioArvores.Services;
using Microsoft.Extensions.Logging;

namespace InventarioArvores.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ArvoresController : ControllerBase
    {
        private readonly IArvoreService _arvoreService;
        private readonly ILogger<ArvoresController> _logger;

        public ArvoresController(IArvoreService arvoreService, ILogger<ArvoresController> logger)
        {
            _arvoreService = arvoreService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] Arvore novaArvore)
        {
            await _arvoreService.CreateAsync(novaArvore);
            return CreatedAtAction(nameof(ObterPorId), new { id = novaArvore.Id }, novaArvore);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArvoreDetalhadaDto>>> ObterTodas()
        {
            var arvores = await _arvoreService.GetAllAsync();
            return Ok(arvores);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<ArvoreDetalhadaDto>> ObterPorId(string id)
        {
            var arvore = await _arvoreService.GetByIdAsync(id);
            if (arvore == null) return NotFound();
            return Ok(arvore);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(string id, [FromBody] Arvore arvoreAtualizada)
        {
            if (!await _arvoreService.UpdateAsync(id, arvoreAtualizada)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(string id)
        {
            if (!await _arvoreService.DeleteAsync(id)) return NotFound();
            return NoContent();
        }
    }
}
