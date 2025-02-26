using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        // Asegurar precisión correcta
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; }

        // Validación para evitar valores no permitidos
        [Required]
        [RegularExpression("^(Ingreso|Gasto)$", ErrorMessage = "El tipo de transacción debe ser 'Ingreso' o 'Gasto'.")]
        public string Tipo { get; set; }

        public string Descripcion { get; set; }
    }
}
