using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class CategoriaGasto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación con Gastos
        public ICollection<Gasto> Gastos { get; set; }
    }
}
