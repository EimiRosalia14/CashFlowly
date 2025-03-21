using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface ICategoriaGastoPersonalizadaRepository
    {
        Task<IEnumerable<CategoriaGastoPersonalizada>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<CategoriaGastoPersonalizada> ObtenerPorIdAsync(int id);
        Task AgregarAsync(CategoriaGastoPersonalizada categoria);
        Task EliminarAsync(CategoriaGastoPersonalizada categoria);
    }
}
