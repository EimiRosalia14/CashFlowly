using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class Gasto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        public int? CategoriaId { get; set; }
        public CategoriaGasto Categoria { get; set; }

        public int? CategoriaPersonalizadaId { get; set; }
        public CategoriaGastoPersonalizada CategoriaPersonalizada { get; set; }

        public int CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
