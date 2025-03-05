using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class CategoriaGastoPersonalizada
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // Relación con el Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
