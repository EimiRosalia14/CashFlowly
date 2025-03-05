using System.Security.Claims;
using CashFlowly.Core.Application.DTOs.Gastos;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GastoController : ControllerBase
    {
        private readonly IGastoService _gastoService;
        private readonly ILogger<GastoController> _logger;

        public GastoController(IGastoService gastoService, ILogger<GastoController> logger)
        {
            _gastoService = gastoService;
            _logger = logger;
        }
        // Obtener el UsuarioId desde el token JWT
        private int GetUsuarioId()
        {
            var userIdClaim = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            if (userIdClaim == null)
            {
                throw new Exception("Usuario no autenticado.");
            }
            return userIdClaim;
        }

        // 1. Registrar un gasto
        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarGasto([FromBody] RegistrarGastoDto gastoDto)
        {
            int usuarioId = GetUsuarioId();
            await _gastoService.RegistrarGastoAsync(gastoDto, usuarioId);
            return Ok(new { message = "Gasto registrado correctamente." });
        }

        // 2. Obtener los gastos del usuario autenticado
        [HttpGet]
        public async Task<IActionResult> ObtenerGastos()
        {
            int usuarioId = GetUsuarioId();
            var gastos = await _gastoService.ObtenerGastosPorUsuarioAsync(usuarioId);
            return Ok(gastos);
        }

        // 3. Editar un gasto
        [HttpPut("editar/{gastoId}")]
        public async Task<IActionResult> EditarGastoAsync(int gastoId, [FromBody] EditarrGastoDto gastoDto)
        {
            try
            {
                // Obtener el usuarioId desde el token JWT (asumimos que el token contiene el usuarioId)
                var usuarioId = int.Parse(User.FindFirst("UserId")?.Value);

                // Asignar el Id del gasto en el DTO para que el servicio lo utilice
                gastoDto.Id = gastoId;

                // Llamar al servicio para editar el gasto
                await _gastoService.EditarGastoAsync(gastoDto, usuarioId);

                return Ok(new { message = "Gasto editado correctamente." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar el gasto.");
                return BadRequest(new { message = ex.Message });
            }
        }


        // 4. Eliminar un gasto
        [HttpDelete("eliminar/{gastoId}")]
        public async Task<IActionResult> EliminarGasto(int gastoId)
        {
            int usuarioId = GetUsuarioId();
            await _gastoService.EliminarGastoAsync(gastoId, usuarioId);
            return Ok(new { message = "Gasto eliminado correctamente." });
        }
    }
}
