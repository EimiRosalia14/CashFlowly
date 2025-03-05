using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFlowly.Core.Application.DTOs.Gastos;
using CashFlowly.Core.Domain.Entities;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Application.Interfaces.Repositories;

namespace CashFlowly.Core.Application.Services.Gasto
{
    public class GastoService : IGastoService
    {
        private readonly IGastosRepository _gastosRepository;
        public GastoService(IGastosRepository gastosRepository)
        {
            _gastosRepository = gastosRepository;
        }
        public async Task RegistrarGastoAsync(RegistrarGastoDto gastoDto, int usuarioId)
        {
            var gasto = new CashFlowly.Core.Domain.Entities.Gasto
            {
                CategoriaId = gastoDto.CategoriaId,
                Monto = gastoDto.Monto,
                Fecha = gastoDto.Fecha,
                UsuarioId = usuarioId,
                CuentaId = gastoDto.CuentaId
            };

            await _gastosRepository.RegistrarGastoAsync(gasto);
        }

        //crea el metodo ObtenerGastosPorUsuarioAsync
        public async Task<List<MostrarGastos>> ObtenerGastosPorUsuarioAsync(int usuarioId)
        {
            var gastos = await _gastosRepository.ObtenerGastosPorUsuarioAsync(usuarioId);
            return gastos.Select(g => new MostrarGastos
            {
                Id = g.Id,
                Monto = g.Monto,
                Fecha = g.Fecha,
                Categoria = g.Categoria.Nombre,
                Cuenta = g.Cuenta.Nombre,
                Usuario = g.Usuario.Nombre
            }).ToList();
        }

        public async Task EditarGastoAsync(RegistrarGastoDto gastoDto, int usuarioId)
        {
            var gasto = await _gastosRepository.ObtenerGastoPorIdAsync(gastoDto.Id);

            if (gasto == null || gasto.UsuarioId != usuarioId)
            {
                throw new Exception("El gasto no existe o no pertenece al usuario.");
            }

            gasto.Monto = gastoDto.Monto;
            gasto.Fecha = gastoDto.Fecha;
            gasto.CategoriaId = gastoDto.CategoriaId;
            gasto.CuentaId = gastoDto.CuentaId;


            await _gastosRepository.EditarGastoAsync(gasto);
        }
        public async Task EliminarGastoAsync(int gastoId, int usuarioId)
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
    }
}
