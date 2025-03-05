using CashFlowly.Core.Application.DTOs.Ingresos;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class IngresosController : ControllerBase
    {
        private readonly IIngresosService _ingresosService;

        public IngresosController(IIngresosService ingresosService)
        {
            _ingresosService = ingresosService;
        }

        private int GetUsuarioId()
        {
            return int.Parse(User.Claims.First(c => c.Type == "id").Value);
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarIngreso([FromBody] RegistrarIngresoDto ingresoDto)
        {
            int usuarioId = GetUsuarioId();
            await _ingresosService.RegistrarIngresoAsync(ingresoDto, usuarioId);
            return Ok(new { message = "Ingreso registrado correctamente." });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerIngresos()
        {
            int usuarioId = GetUsuarioId();
            var ingresos = await _ingresosService.ObtenerIngresosPorUsuarioAsync(usuarioId);
            return Ok(ingresos);
        }
    }
}
