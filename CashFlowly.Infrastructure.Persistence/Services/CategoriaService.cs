using CashFlowly.Core.Application.Interfaces.Repositories;
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
        private readonly ICategoriaRepository<CategoriaIngreso> _categoriaIngresoRepository;
        private readonly ICategoriaRepository<CategoriaGasto> _categoriaGastoRepository;
        private readonly ICategoriaIngresoPersonalizadaRepository _categoriaIngresoPersonalizadaRepository;
        private readonly ICategoriaGastoPersonalizadaRepository _categoriaGastoPersonalizadaRepository;

        public CategoriaService(
            ICategoriaRepository<CategoriaIngreso> categoriaIngresoRepository,
            ICategoriaRepository<CategoriaGasto> categoriaGastoRepository,
            ICategoriaIngresoPersonalizadaRepository categoriaIngresoPersonalizadaRepository,
            ICategoriaGastoPersonalizadaRepository categoriaGastoPersonalizadaRepository)
        {
            _categoriaIngresoRepository = categoriaIngresoRepository;
            _categoriaGastoRepository = categoriaGastoRepository;
            _categoriaIngresoPersonalizadaRepository = categoriaIngresoPersonalizadaRepository;
            _categoriaGastoPersonalizadaRepository = categoriaGastoPersonalizadaRepository;
        }

        // CATEGORÍAS FIJAS
        public async Task<IEnumerable<CategoriaIngreso>> ObtenerTodasFijasIngresosAsync()
        {
            return await _categoriaIngresoRepository.ObtenerTodasAsync();
        }

        public async Task<IEnumerable<CategoriaGasto>> ObtenerTodasFijasGastosAsync()
        {
            return await _categoriaGastoRepository.ObtenerTodasAsync();
        }

        // CATEGORÍAS PERSONALIZADAS
        public async Task<IEnumerable<CategoriaIngresoPersonalizada>> ObtenerPersonalizadasPorUsuarioIngresosAsync(int usuarioId)
        {
            return await _categoriaIngresoPersonalizadaRepository.ObtenerPorUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<CategoriaGastoPersonalizada>> ObtenerPersonalizadasPorUsuarioGastosAsync(int usuarioId)
        {
            return await _categoriaGastoPersonalizadaRepository.ObtenerPorUsuarioAsync(usuarioId);
        }

        public async Task<bool> AgregarCategoriaPersonalizadaIngresosAsync(int usuarioId, string nombre)
        {
            var nuevaCategoria = new CategoriaIngresoPersonalizada
            {
                UsuarioId = usuarioId,
                Nombre = nombre
            };

            await _categoriaIngresoPersonalizadaRepository.AgregarAsync(nuevaCategoria);
            return true;
        }

        public async Task<bool> AgregarCategoriaPersonalizadaGastosAsync(int usuarioId, string nombre)
        {
            var nuevaCategoria = new CategoriaGastoPersonalizada
            {
                UsuarioId = usuarioId,
                Nombre = nombre
            };

            await _categoriaGastoPersonalizadaRepository.AgregarAsync(nuevaCategoria);
            return true;
        }

        public async Task<bool> EliminarCategoriaPersonalizadaIngresosAsync(int id, int usuarioId)
        {
            var categoria = await _categoriaIngresoPersonalizadaRepository.ObtenerPorIdAsync(id);
            if (categoria == null || categoria.UsuarioId != usuarioId)
            {
                return false;
            }

            await _categoriaIngresoPersonalizadaRepository.EliminarAsync(categoria);
            return true;
        }

        public async Task<bool> EliminarCategoriaPersonalizadaGastosAsync(int id, int usuarioId)
        {
            var categoria = await _categoriaGastoPersonalizadaRepository.ObtenerPorIdAsync(id);
            if (categoria == null || categoria.UsuarioId != usuarioId)
            {
                return false;
            }

            await _categoriaGastoPersonalizadaRepository.EliminarAsync(categoria);
            return true;
        }
    }
}
