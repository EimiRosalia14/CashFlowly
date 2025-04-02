using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.DTOs.Metas
{
    public class MostrarMetaDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Objetivo { get; set; }
        public DateTime FechaPropuesta { get; set; }
        public decimal ProgresoActual { get; set; }
    }
}
