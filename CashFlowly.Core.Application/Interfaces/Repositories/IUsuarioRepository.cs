using CashFlowly.Core.Domain.Entities;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task ActualizarAsync(Usuario usuario);
        Task AgregarAsync(Usuario usuario);
        Task IngresarSaldo(Usuario usuario);
        Task<Usuario> ObtenerPorEmailAsync(string email);
        Task<Usuario> ObtenerPorIdAsync(int id);
        Task<Usuario> ObtenerPorTokenAsync(string token);
    }
}