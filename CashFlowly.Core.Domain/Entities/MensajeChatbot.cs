using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class MensajeChatbot
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } // Relación con Usuario

        public string Pregunta { get; set; }
        public string Respuesta { get; set; }
        public DateTime Fecha { get; set; }
    }
}
