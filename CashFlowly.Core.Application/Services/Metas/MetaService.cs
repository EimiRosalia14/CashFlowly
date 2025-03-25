using CashFlowly.Core.Application.DTOs.Metas;
using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Services.Metas
{
    public class MetaService : IMetaService
    {
        private readonly IMetaRepository _metaRepository;
        private readonly IIngresosRepository _ingresosRepository;

        public MetaService(IMetaRepository metaRepository, IIngresosRepository ingresosRepository)
        {
            _metaRepository = metaRepository;
            _ingresosRepository = ingresosRepository;
        }

        public async Task CrearMetaAsync(RegistrarMetaDto dto, int usuarioId)
        {
            var meta = new MetaFinanciera
            {
                Nombre = dto.Nombre,
                Objetivo = dto.Objetivo,
                FechaPropuesta = dto.FechaPropuesta,
                UsuarioId = usuarioId
            };

            await _metaRepository.AddAsync(meta);
        }

        public async Task<List<MostrarMetaDto>> ObtenerMetasPorUsuarioAsync(int usuarioId)
        {
            var metas = await _metaRepository.GetAllByUserAsync(usuarioId);
            var ingresos = await _ingresosRepository.ObtenerIngresosPorUsuarioAsync(usuarioId);

            return metas.Select(meta => new MostrarMetaDto
            {
                Id = meta.Id,
                Nombre = meta.Nombre,
                Objetivo = meta.Objetivo,
                FechaPropuesta = meta.FechaPropuesta,
                ProgresoActual = ingresos
                    .Where(i => i.Fecha <= meta.FechaPropuesta)
                    .Sum(i => i.Monto)
            }).ToList();
        }

        public async Task EliminarMetaAsync(int metaId, int usuarioId)
        {
            var meta = await _metaRepository.GetByIdAsync(metaId);
            if (meta == null || meta.UsuarioId != usuarioId)
                throw new Exception("Meta no encontrada o no pertenece al usuario.");

            await _metaRepository.DeleteAsync(meta);
        }
    }
}
