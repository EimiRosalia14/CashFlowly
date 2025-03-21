using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class CategoriaIngreso
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación con Ingresos
        public ICollection<Ingreso> Ingresos { get; set; }
    }
}
