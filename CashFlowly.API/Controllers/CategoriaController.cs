using CashFlowly.Core.Application.DTOs.Categoria;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Authorize]
    [Route("api/categorias")]
    [ApiController]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        // CATEGORÍAS FIJAS
        [HttpGet("fijas/ingresos")]
        public async Task<IActionResult> ObtenerCategoriasFijasIngresos()
        {
            var categorias = await _categoriaService.ObtenerTodasFijasIngresosAsync();
            return Ok(categorias);
        }

        [HttpGet("fijas/gastos")]
        public async Task<IActionResult> ObtenerCategoriasFijasGastos()
        {
            var categorias = await _categoriaService.ObtenerTodasFijasGastosAsync();
            return Ok(categorias);
        }

        // CATEGORÍAS PERSONALIZADAS
        [HttpGet("personalizadas/ingresos")]
        public async Task<IActionResult> ObtenerCategoriasPersonalizadasIngresos()
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var categorias = await _categoriaService.ObtenerPersonalizadasPorUsuarioIngresosAsync(usuarioId);
            return Ok(categorias);
        }

        [HttpGet("personalizadas/gastos")]
        public async Task<IActionResult> ObtenerCategoriasPersonalizadasGastos()
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var categorias = await _categoriaService.ObtenerPersonalizadasPorUsuarioGastosAsync(usuarioId);
            return Ok(categorias);
        }

        [HttpPost("personalizadas/ingresos")]
        public async Task<IActionResult> AgregarCategoriaPersonalizadaIngresos([FromBody] CrearCategoriaIngresoPersonalizadaDto dto)
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var resultado = await _categoriaService.AgregarCategoriaPersonalizadaIngresosAsync(usuarioId, dto.Nombre);
            if (!resultado)
                return BadRequest("No se pudo agregar la categoría.");
            return Ok("Categoría personalizada de ingresos agregada correctamente.");
        }

        [HttpPost("personalizadas/gastos")]
        public async Task<IActionResult> AgregarCategoriaPersonalizadaGastos([FromBody] CrearCategoriaGastoPersonalizadaDto dto)
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var resultado = await _categoriaService.AgregarCategoriaPersonalizadaGastosAsync(usuarioId, dto.Nombre);
            if (!resultado)
                return BadRequest("No se pudo agregar la categoría.");
            return Ok("Categoría personalizada de gastos agregada correctamente.");
        }

        [HttpDelete("personalizadas/ingresos/{id}")]
        public async Task<IActionResult> EliminarCategoriaPersonalizadaIngresos(int id)
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var resultado = await _categoriaService.EliminarCategoriaPersonalizadaIngresosAsync(id, usuarioId);
            if (!resultado)
                return NotFound("Categoría de ingresos no encontrada o no pertenece al usuario.");
            return Ok("Categoría personalizada de ingresos eliminada correctamente.");
        }

        [HttpDelete("personalizadas/gastos/{id}")]
        public async Task<IActionResult> EliminarCategoriaPersonalizadaGastos(int id)
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var resultado = await _categoriaService.EliminarCategoriaPersonalizadaGastosAsync(id, usuarioId);
            if (!resultado)
                return NotFound("Categoría de gastos no encontrada o no pertenece al usuario.");
            return Ok("Categoría personalizada de gastos eliminada correctamente.");
        }
    }
}