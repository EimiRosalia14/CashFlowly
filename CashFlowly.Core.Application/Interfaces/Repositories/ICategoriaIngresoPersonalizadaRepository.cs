using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface ICategoriaIngresoPersonalizadaRepository
    {
        Task<IEnumerable<CategoriaIngresoPersonalizada>> ObtenerPorUsuarioAsync(int usuarioId);
        Task<CategoriaIngresoPersonalizada> ObtenerPorIdAsync(int id);
        Task AgregarAsync(CategoriaIngresoPersonalizada categoria);
        Task EliminarAsync(CategoriaIngresoPersonalizada categoria);
    }
}
