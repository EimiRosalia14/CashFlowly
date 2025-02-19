using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime FechaRegistro { get; set; }
        public int IntentosFallidos { get; set; } // Contador de intentos fallidos de inicio de sesión

        public bool Bloqueado { get; set; } // Estado de la cuenta (bloqueada o no)
    }
}
