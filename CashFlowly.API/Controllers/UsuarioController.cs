using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Authorize]
    [Route("api/Usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [HttpPost("ingresar-saldo")]
        public async Task<IActionResult> IngresarSaldo([FromQuery] IngresarSaldoDto ingresarSaldoDto)
        {
            var usuarioId = int.Parse(User.Claims.First(c => c.Type == "id").Value);
            var resultado = await _usuarioService.IngresarSaldo(usuarioId, ingresarSaldoDto.SaldoDisponible);
            return Ok(resultado);
        }
    }
}
