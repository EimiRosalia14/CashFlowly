using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class Alerta
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Relación con Usuario

        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public bool Leida { get; set; } = false;
    }
}
