using AutoMapper;
using CashFlowly.Core.Application.DTOs.Ingresos;
using CashFlowly.Core.Application.Interfaces.Repositories.CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Application.Services.Common;
using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace CashFlowly.Core.Application.Services.Ingresos
{
    public class IngresosService : IIngresosService
    {
        private readonly IIngresosRepository _ingresosRepository;
        private readonly ICuentasRepository _cuentasRepository;
        private readonly ILogger<IngresosService> _logger;

        public IngresosService(IIngresosRepository ingresosRepository, ICuentasRepository cuentasRepository, ILogger<IngresosService> logger)
        {
            _ingresosRepository = ingresosRepository;
            _cuentasRepository = cuentasRepository;
            _logger = logger;
        }

        public async Task RegistrarIngresoAsync(RegistrarIngresoDto ingresoDto, int usuarioId)
        {
            if (!ingresoDto.CategoriaId.HasValue && !ingresoDto.CategoriaPersonalizadaId.HasValue)
            {
                throw new Exception("Debe seleccionar al menos una categoría.");
            }

            var cuenta = await _cuentasRepository.GetByIdAsync(ingresoDto.CuentaId);
            if (cuenta == null || cuenta.UsuarioId != usuarioId)
            {
                throw new Exception("Cuenta no válida para el usuario.");
            }

            var ingreso = new Ingreso
            {
                Monto = ingresoDto.Monto,
                Fecha = ingresoDto.Fecha,
                IngresoFijo = ingresoDto.IngresoFijo,
                UsuarioId = usuarioId,
                CuentaId = ingresoDto.CuentaId,
                CategoriaId = ingresoDto.CategoriaId,  // Nombre corregido
                CategoriaPersonalizadaId = ingresoDto.CategoriaPersonalizadaId  // Nombre corregido
            };

            // Sumar el saldo a la cuenta
            cuenta.SaldoDisponible += ingreso.Monto;

            await _ingresosRepository.RegistrarIngresoAsync(ingreso);
            await _cuentasRepository.UpdateAsync(cuenta, cuenta.Id);
        }

        public async Task<List<MostrarIngresos>> ObtenerIngresosPorUsuarioAsync(int usuarioId)
        {
            var ingresos = await _ingresosRepository.ObtenerIngresosPorUsuarioAsync(usuarioId);
            return ingresos.Select(i => new MostrarIngresos
            {
                Id = i.Id,
                Monto = i.Monto,
                Fecha = i.Fecha,
                IngresoFijo = i.IngresoFijo,
                Categoria = i.Categoria?.Nombre,
                Cuenta = i.Cuenta.Nombre,
                CategoriaPersonalizada = i.CategoriaPersonalizada?.Nombre
            }).ToList();
        }

        public async Task EditarIngresoAsync(RegistrarIngresoDto ingresoDto, int ingresoId, int usuarioId)
        {
            var ingreso = await _ingresosRepository.ObtenerIngresoPorIdAsync(ingresoId);
            if (ingreso == null || ingreso.UsuarioId != usuarioId)
            {
                throw new Exception("Ingreso no válido o no pertenece al usuario.");
            }

            // Obtener la cuenta del ingreso
            var cuenta = await _cuentasRepository.GetByIdAsync(ingreso.CuentaId);
            if (cuenta == null)
            {
                throw new Exception("Cuenta no encontrada.");
            }

            // Calcular la diferencia del monto
            decimal diferenciaMonto = ingresoDto.Monto - ingreso.Monto;

            // Ajustar el saldo de la cuenta
            cuenta.SaldoDisponible += diferenciaMonto;

            // Actualizar los datos del ingreso
            ingreso.Monto = ingresoDto.Monto;
            ingreso.Fecha = ingresoDto.Fecha;
            ingreso.CuentaId = ingresoDto.CuentaId;
            ingreso.CategoriaId = ingresoDto.CategoriaId;
            ingreso.CategoriaPersonalizadaId = ingresoDto.CategoriaPersonalizadaId == 0 ? null : ingresoDto.CategoriaPersonalizadaId;

            await _ingresosRepository.EditarIngresoAsync(ingreso);
            await _cuentasRepository.UpdateAsync(cuenta, cuenta.Id); //Guardar cambios en la cuenta
        }

        public async Task EliminarIngresoAsync(int ingresoId, int usuarioId)
        {
            var ingreso = await _ingresosRepository.ObtenerIngresoPorIdAsync(ingresoId);
            if (ingreso == null || ingreso.UsuarioId != usuarioId)
            {
                throw new Exception("Ingreso no válido o no pertenece al usuario.");
            }

            await _ingresosRepository.EliminarIngresoAsync(ingresoId);
        }

        public async Task ProcesarIngresosFijos()
        {
            var today = DateTime.UtcNow.Date;
            var ingresosFijos = await _ingresosRepository.ObtenerIngresosPorUsuarioAsync(0); // Obtener todos

            foreach (var ingreso in ingresosFijos.Where(i => i.IngresoFijo && i.Fecha.Day == today.Day))
            {
                var cuenta = await _cuentasRepository.GetByIdAsync(ingreso.CuentaId);
                if (cuenta != null)
                {
                    var nuevoIngreso = new Ingreso
                    {
                        Monto = ingreso.Monto,
                        IngresoFijo = true,
                        Fecha = ingreso.Fecha.AddMonths(1),
                        CategoriaId = ingreso.CategoriaId,
                        CuentaId = ingreso.CuentaId,
                        UsuarioId = ingreso.UsuarioId
                    };

                    cuenta.SaldoDisponible += ingreso.Monto;
                    await _ingresosRepository.RegistrarIngresoAsync(nuevoIngreso);
                    await _cuentasRepository.UpdateAsync(cuenta, cuenta.Id);
                }
            }
        }
    }
}