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
    public class CategoriaRepository<T> : ICategoriaRepository<T> where T : class
    {
        private readonly CashFlowlyDbContext _context;
        private readonly DbSet<T> _dbSet;

        public CategoriaRepository(CashFlowlyDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<IEnumerable<T>> ObtenerTodasAsync()
        {
            return await _dbSet.ToListAsync();
        }
    }
}
