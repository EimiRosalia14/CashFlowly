using CashFlowly.Core.Application.Interfaces.Repositories.CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Domain.Entities;
using CashFlowly.Infrastructure.Persistence.Contexts;

namespace CashFlowly.Infrastructure.Persistence.Repositories
{
    public class CuentasRepository : GenericRepository<Cuenta>, ICuentasRepository
    {
        public CuentasRepository(CashFlowlyDbContext context) : base(context)
        {
        }
    }
}
