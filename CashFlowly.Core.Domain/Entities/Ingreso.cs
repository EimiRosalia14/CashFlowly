using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class Ingreso
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public bool IngresoFijo { get; set; }
        public DateTime Fecha { get; set; }

        // Categoría fija (opcional)
        public int? CategoriaId { get; set; }
        public CategoriaIngreso? Categoria { get; set; }

        // Categoría personalizada (opcional)
        public int? CategoriaPersonalizadaId { get; set; }  // Nombre corregido
        public CategoriaIngresoPersonalizada? CategoriaPersonalizada { get; set; }

        public int CuentaId { get; set; }
        public Cuenta Cuenta { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

    }
}
