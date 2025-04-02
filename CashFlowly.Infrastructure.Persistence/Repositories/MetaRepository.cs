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
    public class MetaRepository : IMetaRepository
    {
        private readonly CashFlowlyDbContext _context;

        public MetaRepository(CashFlowlyDbContext context)
        {
            _context = context;
        }

        public async Task<List<MetaFinanciera>> GetAllByUserAsync(int usuarioId)
        {
            return await _context.MetasFinancieras.Where(m => m.UsuarioId == usuarioId).ToListAsync();
        }

        public async Task<MetaFinanciera> GetByIdAsync(int id)
        {
            return await _context.MetasFinancieras.FindAsync(id);
        }

        public async Task AddAsync(MetaFinanciera meta)
        {
            await _context.MetasFinancieras.AddAsync(meta);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MetaFinanciera meta)
        {
            _context.MetasFinancieras.Update(meta);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(MetaFinanciera meta)
        {
            _context.MetasFinancieras.Remove(meta);
            await _context.SaveChangesAsync();
        }
    }
}
