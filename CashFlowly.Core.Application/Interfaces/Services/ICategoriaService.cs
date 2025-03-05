using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface ICategoriaService
    {
        // CATEGORÍAS FIJAS
        Task<IEnumerable<CategoriaIngreso>> ObtenerTodasFijasIngresosAsync();
        Task<IEnumerable<CategoriaGasto>> ObtenerTodasFijasGastosAsync();

        // CATEGORÍAS PERSONALIZADAS
        Task<IEnumerable<CategoriaIngresoPersonalizada>> ObtenerPersonalizadasPorUsuarioIngresosAsync(int usuarioId);
        Task<IEnumerable<CategoriaGastoPersonalizada>> ObtenerPersonalizadasPorUsuarioGastosAsync(int usuarioId);

        Task<bool> AgregarCategoriaPersonalizadaIngresosAsync(int usuarioId, string nombre);
        Task<bool> AgregarCategoriaPersonalizadaGastosAsync(int usuarioId, string nombre);

        Task<bool> EliminarCategoriaPersonalizadaIngresosAsync(int id, int usuarioId);
        Task<bool> EliminarCategoriaPersonalizadaGastosAsync(int id, int usuarioId);
    }

}
