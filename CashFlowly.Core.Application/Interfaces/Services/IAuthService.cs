using CashFlowly.Core.Application.DTOs.Usuario;
using CashFlowly.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Usuario> GetUserByIdAsync(int id);
        Task<Usuario> GetUsuarioByEmailAsync(string email);
        Task<string> RegistrarUsuarioAsync(UsuarioRegistroDto usuarioDto);
        Task<string> LoginAsync(LoginDto loginDto);

        Task<bool> ConfirmarCuentaAsync(string token);
    }
}
