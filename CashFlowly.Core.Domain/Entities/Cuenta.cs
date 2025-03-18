using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class Cuenta
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string NumeroDeCuenta { get; set; }
        public decimal SaldoDisponible { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
    }
}
