﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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
        public bool Suspendido { get; set; } // Estado de la cuenta (bloqueada o no)

        // Propiedades necesarias para la verificación de cuenta
        public bool Confirmado { get; set; } = false;
        public bool RecordarSesion { get; set; }
        public string? TokenVerificacion { get; set; }

        // Nuevo: Saldo Disponible para gastar
        public decimal SaldoDisponible { get; set; } = 0;

        // Relaciones
        public ICollection<Transaccion> Transacciones { get; set; }
        public ICollection<MetaFinanciera> MetasFinancieras { get; set; }
        public ICollection<Alerta> Alertas { get; set; }
    }
}
