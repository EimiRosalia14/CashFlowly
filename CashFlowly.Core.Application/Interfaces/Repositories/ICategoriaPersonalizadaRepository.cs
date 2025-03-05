using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface ICategoriaPersonalizadaRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<T> ObtenerPorIdAsync(int id);
        Task AgregarAsync(T entidad);
        Task EliminarAsync(T entidad);
    }
}
