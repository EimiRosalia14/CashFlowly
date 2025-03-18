using CashFlowly.Core.Application.Interfaces.Repositories;
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
    public class CategoriaIngresoPersonalizadaRepository : ICategoriaIngresoPersonalizadaRepository
    {
        private readonly CashFlowlyDbContext _context;
        private readonly DbSet<CategoriaIngresoPersonalizada> _dbSet;

        public CategoriaIngresoPersonalizadaRepository(CashFlowlyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<CategoriaIngresoPersonalizada>();
        }

        public async Task<IEnumerable<CategoriaIngresoPersonalizada>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet.Where(c => c.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<CategoriaIngresoPersonalizada> ObtenerPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AgregarAsync(CategoriaIngresoPersonalizada categoria)
        {
            await _dbSet.AddAsync(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(CategoriaIngresoPersonalizada categoria)
        {
            _dbSet.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
