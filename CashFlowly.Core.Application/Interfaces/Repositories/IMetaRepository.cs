using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface IMetaRepository
    {
        Task<List<MetaFinanciera>> GetAllByUserAsync(int usuarioId);
        Task<MetaFinanciera> GetByIdAsync(int id);
        Task AddAsync(MetaFinanciera meta);
        Task UpdateAsync(MetaFinanciera meta);
        Task DeleteAsync(MetaFinanciera meta);
    }
}
