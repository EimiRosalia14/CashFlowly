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
    public class CategoriaGastoPersonalizadaRepository : ICategoriaGastoPersonalizadaRepository
    {
        private readonly CashFlowlyDbContext _context;
        private readonly DbSet<CategoriaGastoPersonalizada> _dbSet;

        public CategoriaGastoPersonalizadaRepository(CashFlowlyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<CategoriaGastoPersonalizada>();
        }

        public async Task<IEnumerable<CategoriaGastoPersonalizada>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet.Where(c => c.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<CategoriaGastoPersonalizada> ObtenerPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AgregarAsync(CategoriaGastoPersonalizada categoria)
        {
            await _dbSet.AddAsync(categoria);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(CategoriaGastoPersonalizada categoria)
        {
            _dbSet.Remove(categoria);
            await _context.SaveChangesAsync();
        }
    }
}
