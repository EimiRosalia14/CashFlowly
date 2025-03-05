using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.DTOs.Gastos
{
    public class RegistrarGastoDto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public int CategoriaId { get; set; }
        public int CuentaId { get; set; }
        public int UsuarioId { get; set; }

    }
}
