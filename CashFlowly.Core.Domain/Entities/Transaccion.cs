using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Relación con Usuario

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; } // Relación con Categoría

        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }

        // Puede ser "Ingreso" o "Gasto"
        public string Tipo { get; set; }

        public string Descripcion { get; set; }
    }
}
