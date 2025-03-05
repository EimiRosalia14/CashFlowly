using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlowly.Core.Application.DTOs.Gastos;
using CashFlowly.Core.Domain.Entities;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace CashFlowly.Core.Application.Services.Gasto
{
    public class GastoService : IGastoService
    {
        private readonly IGastosRepository _gastosRepository;
        private readonly ILogger<GastoService> _logger;

        public GastoService(IGastosRepository gastosRepository, ILogger<GastoService> logger)
        {
            _gastosRepository = gastosRepository;
            _logger = logger;
        }

        public async Task RegistrarGastoAsync(RegistrarGastoDto gastoDto, int usuarioId)
        {
            try
            {
                // Validación: Debe seleccionar al menos una categoría
                if (!gastoDto.CategoriaGastoId.HasValue && !gastoDto.CategoriaGastoPersonalizadoId.HasValue)
                {
                    throw new Exception("Debe seleccionar al menos una categoría.");
                }

                // Si alguna categoría es 0, asignarla como null
                var categoriaId = gastoDto.CategoriaGastoId == 0 ? (int?)null : gastoDto.CategoriaGastoId;
                var categoriaIdP = gastoDto.CategoriaGastoPersonalizadoId == 0 ? (int?)null : gastoDto.CategoriaGastoPersonalizadoId;

                var gasto = new CashFlowly.Core.Domain.Entities.Gasto
                {
                    Monto = gastoDto.Monto,
                    Fecha = gastoDto.Fecha,
                    UsuarioId = usuarioId,
                    CuentaId = gastoDto.CuentaId,
                    CategoriaId = categoriaId,  // Asignar null si es 0
                    CategoriaPersonalizadaId = categoriaIdP  // Asignar null si es 0
                };

                await _gastosRepository.RegistrarGastoAsync(gasto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar gasto");
                throw new Exception("Ocurrió un error al registrar el gasto.");
            }
        }


        public async Task<List<MostrarGastos>> ObtenerGastosPorUsuarioAsync(int usuarioId)
        {
            try
            {
                var gastos = await _gastosRepository.ObtenerGastosPorUsuarioAsync(usuarioId);
                return gastos.Select(g => new MostrarGastos
                {
                    Id = g.Id,
                    Monto = g.Monto,
                    Fecha = g.Fecha,
                    Categoria = g.Categoria.Nombre,
                    Cuenta = g.Cuenta.Nombre,
                    CategoriaPersonalizada = g.CategoriaPersonalizada?.Nombre ?? "Sin categoría personalizada"

                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener gastos del usuario {UsuarioId}", usuarioId);
                throw new Exception("Ocurrió un error al obtener los gastos.");
            }
        }

        public async Task EditarGastoAsync(EditarrGastoDto gastoDto, int usuarioId)
        {
            try
            {
                if (gastoDto.CategoriaGastoId.HasValue && gastoDto.CategoriaGastoPersonalizadoId.HasValue)
                {
                    throw new Exception("Solo se puede seleccionar una categoría: estándar o personalizada.");
                }

                if (!gastoDto.CategoriaGastoId.HasValue && !gastoDto.CategoriaGastoPersonalizadoId.HasValue)
                {
                    throw new Exception("Debe seleccionar al menos una categoría.");
                }

                var gasto = await _gastosRepository.ObtenerGastoPorIdAsync(gastoDto.Id);

                if (gasto == null)
                {
                    throw new Exception("El gasto no existe.");
                }

                if (gasto.UsuarioId != usuarioId)
                {
                    throw new Exception("No tienes permiso para editar este gasto.");
                }

                // Si alguna categoría es 0, asignarla como null
                var categoriaId = gastoDto.CategoriaGastoId == 0 ? (int?)null : gastoDto.CategoriaGastoId;
                var categoriaIdP = gastoDto.CategoriaGastoPersonalizadoId == 0 ? (int?)null : gastoDto.CategoriaGastoPersonalizadoId;

                gasto.Monto = gastoDto.Monto;
                gasto.Fecha = gastoDto.Fecha;
                gasto.CuentaId = gastoDto.CuentaId;

                // Solo se actualiza un tipo de categoría
                gasto.CategoriaId = categoriaId;
                gasto.CategoriaPersonalizadaId = categoriaIdP;

                await _gastosRepository.EditarGastoAsync(gasto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar gasto {GastoId} del usuario {UsuarioId}", gastoDto.Id, usuarioId);
                throw new Exception("Ocurrió un error al editar el gasto.");
            }
        }

        public async Task EliminarGastoAsync(int gastoId, int usuarioId)
        {
            try
            {
                var gasto = await _gastosRepository.ObtenerGastoPorIdAsync(gastoId);

                if (gasto == null)
                {
                    throw new Exception("El gasto no existe.");
                }

                if (gasto.UsuarioId != usuarioId)
                {
                    throw new Exception("No tienes permiso para eliminar este gasto.");
                }

                await _gastosRepository.EliminarGastoAsync(gastoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar gasto {GastoId} del usuario {UsuarioId}", gastoId, usuarioId);
                throw new Exception("Ocurrió un error al eliminar el gasto.");
            }
        }
    }
}
