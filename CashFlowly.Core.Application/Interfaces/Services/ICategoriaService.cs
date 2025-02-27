using CashFlowly.Core.Application.DTOs.Categoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync();
        Task<CategoriaDto> CrearCategoriaAsync(CrearCategoriaDto categoriaDto);
        Task<bool> EliminarCategoriaAsync(int id);
    }
}
