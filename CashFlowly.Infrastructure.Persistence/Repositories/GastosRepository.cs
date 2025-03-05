using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Domain.Entities;
using CashFlowly.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CashFlowly.Infrastructure.Persistence.Repositories
{
    public class GastosRepository : IGastosRepository
    {
        private readonly CashFlowlyDbContext _context;

        public GastosRepository(CashFlowlyDbContext context)
        {
            _context = context;
        }

        public async Task<Gasto> RegistrarGastoAsync(Gasto gasto)
        {
            await _context.Set<Gasto>().AddAsync(gasto);
            await _context.SaveChangesAsync();
            return gasto;
        }

        public async Task<List<Gasto>> ObtenerGastosPorUsuarioAsync(int usuarioId)
        {
            return await _context.Gastos
                .Where(g => g.UsuarioId == usuarioId)
                .ToListAsync();
        }

        //crea un metodo que edite los gastos
        public async Task<Gasto> ObtenerGastoPorIdAsync(int gastoId)
        {
            return await _context.Gastos.FindAsync(gastoId);
        }

        public async Task EditarGastoAsync(Gasto gasto)
        {
            var gastoExistente = await _context.Gastos.FindAsync(gasto.Id);

            if (gastoExistente != null)
            {
                gastoExistente.Monto = gasto.Monto;
                gastoExistente.Fecha = gasto.Fecha;
                gastoExistente.CategoriaId = gasto.CategoriaId;
                gastoExistente.CuentaId = gasto.CuentaId;

                await _context.SaveChangesAsync();
            }
        }

        public async Task EliminarGastoAsync(int gastoId)
        {
            var gasto = await _context.Gastos.FindAsync(gastoId);
            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
                await _context.SaveChangesAsync();
            }
        }
    }
}
