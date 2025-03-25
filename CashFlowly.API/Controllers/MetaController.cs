using CashFlowly.Core.Application.DTOs.Metas;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MetaController : ControllerBase
    {
        private readonly IMetaService _metaService;

        public MetaController(IMetaService metaService)
        {
            _metaService = metaService;
        }

        private int GetUsuarioId() =>
            int.Parse(User.Claims.First(c => c.Type == "id").Value);

        [HttpPost("crear")]
        public async Task<IActionResult> Crear([FromBody] RegistrarMetaDto dto)
        {
            await _metaService.CrearMetaAsync(dto, GetUsuarioId());
            return Ok(new { message = "Meta registrada correctamente" });
        }

        [HttpGet]
        public async Task<IActionResult> Obtener()
        {
            var metas = await _metaService.ObtenerMetasPorUsuarioAsync(GetUsuarioId());
            return Ok(metas);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            await _metaService.EliminarMetaAsync(id, GetUsuarioId());
            return Ok(new { message = "Meta eliminada correctamente" });
        }
    }
}
