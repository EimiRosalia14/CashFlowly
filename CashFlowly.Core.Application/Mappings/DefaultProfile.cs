using AutoMapper;
using CashFlowly.Core.Application.DTOs.Cuentas;
using CashFlowly.Core.Domain.Entities;

namespace CashFlowly.Core.Application.Mappings
{
    public class DefaultProfile : Profile
    {
        public DefaultProfile() 
        {
            CreateMap<Cuenta, CuentaResponse>()
                .ReverseMap();
            CreateMap<Cuenta, UpdateCuentaDTO>()
                .ReverseMap();
            CreateMap<Cuenta, CreateCuentaDTO>()
                .ReverseMap();
        }
    }
}
