using CashFlowly.Core.Application.DTOs.Transaccion;
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
    public class TransaccionService : ITransaccionService
    {
        private readonly TransaccionRepository _transaccionRepository;
        private readonly UsuarioRepository _usuarioRepository;

        public TransaccionService(TransaccionRepository transaccionRepository, UsuarioRepository usuarioRepository)
        {
            _transaccionRepository = transaccionRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<TransaccionDto>> ObtenerTransaccionesPorUsuarioAsync(int usuarioId)
        {
            var transacciones = await _transaccionRepository.ObtenerPorUsuarioAsync(usuarioId);
            return transacciones.Select(t => new TransaccionDto
            {
                Id = t.Id,
                UsuarioId = t.UsuarioId,
                CategoriaId = t.CategoriaId,
                Monto = t.Monto,
                Fecha = t.Fecha,
                Tipo = t.Tipo,
                Descripcion = t.Descripcion
            }).ToList();
        }

        public async Task<TransaccionDto> ObtenerTransaccionPorIdAsync(int id)
        {
            var transaccion = await _transaccionRepository.ObtenerPorIdAsync(id);
            if (transaccion == null) return null;

            return new TransaccionDto
            {
                Id = transaccion.Id,
                UsuarioId = transaccion.UsuarioId,
                CategoriaId = transaccion.CategoriaId,
                Monto = transaccion.Monto,
                Fecha = transaccion.Fecha,
                Tipo = transaccion.Tipo,
                Descripcion = transaccion.Descripcion
            };
        }

        public async Task<bool> RegistrarTransaccionAsync(int usuarioId, CrearTransaccionDto transaccionDto)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(usuarioId);
            if (usuario == null) return false;

            if (transaccionDto.Tipo == "Gasto" && usuario.SaldoDisponible < transaccionDto.Monto)
                return false; // No permitir gasto si el saldo es insuficiente

            var transaccion = new Transaccion
            {
                UsuarioId = usuarioId,
                CategoriaId = transaccionDto.CategoriaId,
                Monto = transaccionDto.Monto,
                Fecha = DateTime.UtcNow,
                Tipo = transaccionDto.Tipo,
                Descripcion = transaccionDto.Descripcion
            };

            await _transaccionRepository.AgregarAsync(transaccion);

            if (transaccionDto.Tipo == "Gasto")
                usuario.SaldoDisponible -= transaccionDto.Monto;
            else
                usuario.SaldoDisponible += transaccionDto.Monto;

            await _usuarioRepository.ActualizarAsync(usuario);
            return true;
        }

        public async Task<bool> EliminarTransaccionAsync(int id)
        {
            var transaccion = await _transaccionRepository.ObtenerPorIdAsync(id);
            if (transaccion == null) return false;

            await _transaccionRepository.EliminarAsync(transaccion);
            return true;
        }
    }
}
