using AutoMapper;
using CashFlowly.Core.Application.DTOs.Cuentas;
using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Repositories.CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Application.Services.Common;
using CashFlowly.Core.Domain.Entities;

namespace CashFlowly.Core.Application.Services.Cuentas
{
    public class CuentasService : GenericService<CreateCuentaDTO, UpdateCuentaDTO, Cuenta, CuentaResponse>, ICuentasService
    {
        private readonly ICuentasRepository _repo;
        private readonly IMapper _mapper;
        public CuentasService(ICuentasRepository repo, IMapper mapper) : base(repo, mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CuentaResponse>> GetByUserId(int userId)
        {
            var result = await _repo.FindAllAsync(x => x.UsuarioId == userId);
            return _mapper.Map<List<CuentaResponse>>(result);
        }
    }
}
