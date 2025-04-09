using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CashFlowly.Core.Application.DTOs.Gastos;
using CashFlowly.Core.Domain.Entities;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;
using CashFlowly.Core.Application.Interfaces.Repositories.CashFlowly.Core.Application.Interfaces.Repositories;

namespace CashFlowly.Core.Application.Services.Gasto
{
    public class GastoService : IGastoService
    {
        private readonly IGastosRepository _gastosRepository;
        private readonly ILogger<GastoService> _logger;
        private readonly ICuentasRepository _cuentasRepository;

        public GastoService(IGastosRepository gastosRepository, ILogger<GastoService> logger, ICuentasRepository cuentasRepository)
        {
            _gastosRepository = gastosRepository;
            _logger = logger;
            _cuentasRepository = cuentasRepository;
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

                var cuenta = await _cuentasRepository.GetByIdAsync(gastoDto.CuentaId);
                if (cuenta == null || cuenta.UsuarioId != usuarioId)
                {
                    throw new Exception("Cuenta no válida para el usuario.");
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

                // restar el saldo a la cuenta
                cuenta.SaldoDisponible -= gasto.Monto;

                await _gastosRepository.RegistrarGastoAsync(gasto);
                await _cuentasRepository.UpdateAsync(cuenta, cuenta.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al registrar gasto error: {ex.Message}");
                throw new Exception($"Ocurrió un error al registrar el gasto: {ex.Message}");
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
                    Categoria = g.Categoria?.Nombre,
                    Cuenta = g.Cuenta?.Nombre,
                    CategoriaPersonalizada = g.CategoriaPersonalizada?.Nombre ?? "Sin categoría personalizada"

                }).ToList();
            }
            catch (Exception ex)
            {
                string message = $"Error al obtener gastos del usuario {usuarioId}: {ex.Message}";
                _logger.LogError(message);
                throw new Exception($"Ocurrió un error al obtener los gastos: {ex.Message}");
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

                var cuenta = await _cuentasRepository.GetByIdAsync(gasto.CuentaId);
                if (cuenta == null)
                {
                    throw new Exception("Cuenta no encontrada.");
                }

                // Calcular la diferencia del monto
                decimal diferenciaMonto = gastoDto.Monto - gasto.Monto;

                // Ajustar el saldo de la cuenta
                cuenta.SaldoDisponible -= diferenciaMonto;

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
                await _cuentasRepository.UpdateAsync(cuenta, cuenta.Id); //Guardar cambios en la cuenta
                
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al editar gasto {gastoDto.Id} del usuario {usuarioId}");
                throw new Exception($"Ocurrió un error al editar el gasto: {ex.Message}");
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

                var cuenta = await _cuentasRepository.GetByIdAsync(gasto.CuentaId);
                if (cuenta == null || cuenta.UsuarioId != usuarioId)
                {
                    throw new Exception("Cuenta no válida para el usuario.");
                }
                // Restar el monto del ingreso eliminado al saldo de la cuenta
                cuenta.SaldoDisponible += gasto.Monto;

                await _gastosRepository.EliminarGastoAsync(gastoId);
                await _cuentasRepository.UpdateAsync(cuenta, cuenta.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al eliminar gasto {gastoId} del usuario {usuarioId}: {ex.Message}");
                throw new Exception("Ocurrió un error al eliminar el gasto.");
            }
        }
    }
}
