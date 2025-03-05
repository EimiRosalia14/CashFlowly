using CashFlowly.Core.Application.DTOs.Gastos;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IGastoService
    {
        Task EditarGastoAsync(EditarrGastoDto gastoDto, int usuarioId);
        Task EliminarGastoAsync(int gastoId, int usuarioId);
        Task<List<MostrarGastos>> ObtenerGastosPorUsuarioAsync(int usuarioId);
        Task RegistrarGastoAsync(RegistrarGastoDto gastoDto, int usuarioId);
    }
}