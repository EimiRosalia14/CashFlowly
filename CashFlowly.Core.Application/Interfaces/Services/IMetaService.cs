using CashFlowly.Core.Application.DTOs.Metas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IMetaService
    {
        Task CrearMetaAsync(RegistrarMetaDto dto, int usuarioId);
        Task<List<MostrarMetaDto>> ObtenerMetasPorUsuarioAsync(int usuarioId);
        Task EliminarMetaAsync(int metaId, int usuarioId);
    }
}
