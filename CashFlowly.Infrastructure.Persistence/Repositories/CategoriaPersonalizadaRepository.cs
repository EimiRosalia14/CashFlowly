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
    public class CategoriaPersonalizadaRepository<T> : ICategoriaPersonalizadaRepository<T> where T : class
    {
        private readonly CashFlowlyDbContext _context;
        private readonly DbSet<T> _dbSet;

        public CategoriaPersonalizadaRepository(CashFlowlyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> ObtenerPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet.Where(c => EF.Property<int>(c, "UsuarioId") == usuarioId).ToListAsync();
        }

        public async Task<T> ObtenerPorIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AgregarAsync(T entidad)
        {
            await _dbSet.AddAsync(entidad);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarAsync(T entidad)
        {
            _dbSet.Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }
}