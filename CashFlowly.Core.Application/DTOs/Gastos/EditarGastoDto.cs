using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.DTOs.Gastos
{
    public class EditarrGastoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int? CategoriaGastoId { get; set; } // Puede ser null
        public int? CategoriaGastoPersonalizadoId { get; set; } // Puede ser null
        public int CuentaId { get; set; }

    }
}
