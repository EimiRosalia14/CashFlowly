using CashFlowly.Core.Application.DTOs.Categoria;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Route("api/categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var categorias = await _categoriaService.ObtenerTodasAsync();
            return Ok(categorias);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearCategoriaDto categoriaDto)
        {
            var categoria = await _categoriaService.CrearCategoriaAsync(categoriaDto);
            return CreatedAtAction(nameof(ObtenerTodas), new { id = categoria.Id }, categoria);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _categoriaService.EliminarCategoriaAsync(id);
            if (!eliminado) return NotFound("Categoría no encontrada.");
            return Ok("Categoría eliminada correctamente.");
        }
    }
}