using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface ICategoriaRepository<T> where T : class
    {
        Task<IEnumerable<T>> ObtenerTodasAsync();
    }
}
