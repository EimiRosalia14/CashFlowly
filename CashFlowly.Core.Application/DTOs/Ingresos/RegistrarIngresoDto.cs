using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.DTOs.Ingresos
{
    public class RegistrarIngresoDto
    {
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public bool IngresoFijo { get; set; }
        public int? CategoriaId { get; set; } // Nombre corregido
        public int? CategoriaPersonalizadaId { get; set; } // Nombre corregido
        public int CuentaId { get; set; }
    }
}
