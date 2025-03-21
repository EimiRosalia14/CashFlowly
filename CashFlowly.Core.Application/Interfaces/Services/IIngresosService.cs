using CashFlowly.Core.Application.DTOs.Ingresos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IIngresosService
    {
        Task RegistrarIngresoAsync(RegistrarIngresoDto ingresoDto, int usuarioId);
        Task<List<MostrarIngresos>> ObtenerIngresosPorUsuarioAsync(int usuarioId);
        Task EditarIngresoAsync(RegistrarIngresoDto ingresoDto, int ingresoId, int usuarioId);
        Task EliminarIngresoAsync(int ingresoId, int usuarioId);
        Task ProcesarIngresosFijos();
    }
}
