using CashFlowly.Core.Domain.Entities;
using CashFlowly.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Infrastructure.Persistence.Repositories
{
    public class TransaccionRepository
    {
        private readonly CashFlowlyDbContext _context;

        public TransaccionRepository(CashFlowlyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaccion>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _context.Transacciones
                .Where(t => t.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<Transaccion> ObtenerPorIdAsync(int id)
        {
            return await _context.Transacciones.FindAsync(id);
        }

        public async Task AgregarAsync(Transaccion transaccion)
        {
            _context.Transacciones.Add(transaccion);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(Transaccion transaccion)
        {
            _context.Transacciones.Remove(transaccion);
            await _context.SaveChangesAsync();
        }
    }
}
