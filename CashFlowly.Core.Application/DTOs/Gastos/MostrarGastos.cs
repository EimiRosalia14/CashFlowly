using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.DTOs.Gastos
{
    public class MostrarGastos
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Categoria { get; set; }
        public string Cuenta { get; set; }
        public string Usuario { get; set; }
    }
}
