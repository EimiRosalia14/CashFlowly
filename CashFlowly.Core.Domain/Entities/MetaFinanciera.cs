using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class MetaFinanciera
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Relación con Usuario

        public string Nombre { get; set; }
        public decimal MontoObjetivo { get; set; }
        public decimal MontoAhorrado { get; set; }
        public DateTime FechaLimite { get; set; }
        public bool Alcanzada { get; set; }
    }
}
