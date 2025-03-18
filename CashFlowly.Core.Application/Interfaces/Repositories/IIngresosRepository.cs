using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Repositories
{
    public interface IIngresosRepository
    {
        Task RegistrarIngresoAsync(Ingreso ingreso);
        Task<List<Ingreso>> ObtenerIngresosPorUsuarioAsync(int usuarioId);
        Task<Ingreso> ObtenerIngresoPorIdAsync(int ingresoId);
        Task EditarIngresoAsync(Ingreso ingreso);
        Task EliminarIngresoAsync(int ingresoId);
    }
}
