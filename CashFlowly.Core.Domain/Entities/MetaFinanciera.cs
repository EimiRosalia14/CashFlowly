﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Domain.Entities
{
    public class MetaFinanciera
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Objetivo { get; set; }
        public DateTime FechaPropuesta { get; set; }

        // Relación con Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
