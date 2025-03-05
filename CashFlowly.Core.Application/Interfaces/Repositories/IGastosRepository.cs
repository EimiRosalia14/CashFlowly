using CashFlowly.Core.Domain.Entities;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface IGastosRepository
    {
        Task EditarGastoAsync(Gasto gasto);
        Task EliminarGastoAsync(int gastoId);
        Task<Gasto> ObtenerGastoPorIdAsync(int gastoId);
        Task<List<Gasto>> ObtenerGastosPorUsuarioAsync(int usuarioId);
        Task<Gasto> RegistrarGastoAsync(Gasto gasto);
    }
}