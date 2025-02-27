using CashFlowly.Core.Application.DTOs.Categoria;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Domain.Entities;
using CashFlowly.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Infrastructure.Persistence.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly CategoriaRepository _categoriaRepository;

        public CategoriaService(CategoriaRepository categoriaRepository)
        {
            _categoriaRepository = categoriaRepository;
        }

        public async Task<IEnumerable<CategoriaDto>> ObtenerTodasAsync()
        {
            var categorias = await _categoriaRepository.ObtenerTodasAsync();
            return categorias.Select(c => new CategoriaDto { Id = c.Id, Nombre = c.Nombre }).ToList();
        }

        public async Task<CategoriaDto> CrearCategoriaAsync(CrearCategoriaDto categoriaDto)
        {
            var categoria = new Categoria { Nombre = categoriaDto.Nombre };
            var creada = await _categoriaRepository.AgregarAsync(categoria);
            return new CategoriaDto { Id = creada.Id, Nombre = creada.Nombre };
        }

        public async Task<bool> EliminarCategoriaAsync(int id)
        {
            return await _categoriaRepository.EliminarAsync(id);
        }
    }
}
