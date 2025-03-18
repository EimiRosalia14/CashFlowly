using CashFlowly.Core.Application.DTOs.Cuentas;
using CashFlowly.Core.Domain.Entities;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface ICuentasService : IGenericService<CreateCuentaDTO, UpdateCuentaDTO, Cuenta, CuentaResponse>
    {
        Task<List<CuentaResponse>> GetByUserId(int userId);
    }
}
