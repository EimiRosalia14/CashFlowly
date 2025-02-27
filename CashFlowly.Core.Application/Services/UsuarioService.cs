using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Services;

namespace CashFlowly.Core.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<string> IngresarSaldo(int id, decimal saldo)
        {
            var usuario = await _usuarioRepository.ObtenerPorIdAsync(id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado.");
            usuario.SaldoDisponible += saldo;
            await _usuarioRepository.IngresarSaldo(usuario);
            return "Saldo ingresado correctamente.";
        }
    }
}
