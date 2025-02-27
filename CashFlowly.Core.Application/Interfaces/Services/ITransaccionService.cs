using CashFlowly.Core.Application.DTOs.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface ITransaccionService
    {
        Task<IEnumerable<TransaccionDto>> ObtenerTransaccionesPorUsuarioAsync(int usuarioId);
        Task<TransaccionDto> ObtenerTransaccionPorIdAsync(int id);
        Task<bool> RegistrarTransaccionAsync(int usuarioId, CrearTransaccionDto transaccionDto); 
        Task<bool> EliminarTransaccionAsync(int id);
    }
}
