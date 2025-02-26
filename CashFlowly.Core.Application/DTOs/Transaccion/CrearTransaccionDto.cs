using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.DTOs.Transaccion
{
    public class CrearTransaccionDto
    {
        public int CategoriaId { get; set; }
        public decimal Monto { get; set; }

        [RegularExpression("^(Ingreso|Gasto)$", ErrorMessage = "El tipo debe ser 'Ingreso' o 'Gasto'.")]
        public string Tipo { get; set; } // Solo puede ser "Ingreso" o "Gasto"

        public string Descripcion { get; set; }
    }
}
