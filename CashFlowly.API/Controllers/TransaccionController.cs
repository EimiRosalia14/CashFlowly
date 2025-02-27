using CashFlowly.Core.Application.DTOs.Transaccion;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Authorize]
    [Route("api/transacciones")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        private readonly ITransaccionService _transaccionService;

        public TransaccionController(ITransaccionService transaccionService)
        {
            _transaccionService = transaccionService;
        }

        [HttpGet("mis-transacciones")]
        public async Task<IActionResult> ObtenerMisTransacciones()
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var transacciones = await _transaccionService.ObtenerTransaccionesPorUsuarioAsync(usuarioId);
            return Ok(transacciones);
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarTransaccion([FromBody] CrearTransaccionDto transaccionDto)
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value); // Extrae el ID del token

            var resultado = await _transaccionService.RegistrarTransaccionAsync(usuarioId, transaccionDto);

            if (!resultado) return BadRequest("Error al registrar transacción.");

            return Ok("Transacción registrada correctamente.");
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarTransaccion(int id)
        {
            var resultado = await _transaccionService.EliminarTransaccionAsync(id);
            if (!resultado) return NotFound("Transacción no encontrada.");
            return Ok("Transacción eliminada.");
        }
    }
}
