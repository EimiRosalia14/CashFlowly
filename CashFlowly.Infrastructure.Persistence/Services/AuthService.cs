using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashFlowly.Core.Application.DTOs.Usuario;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Infrastructure.Persistence.Contexts;
using CashFlowly.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CashFlowly.Infrastructure.Persistence.Repositories;

namespace CashFlowly.Infrastructure.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly CashFlowlyDbContext _context;

        public AuthService(UsuarioRepository usuarioRepository, IConfiguration configuration, CashFlowlyDbContext context)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _context = context;
        }

        public async Task<string> RegistrarUsuarioAsync(UsuarioRegistroDto usuarioDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var usuarioExistente = await _usuarioRepository.ObtenerPorEmailAsync(usuarioDto.Email);
                    if (usuarioExistente != null)
                    {
                        throw new Exception("El correo ya está en uso.");
                    }

                    var usuario = new Usuario
                    {
                        Nombre = usuarioDto.Nombre,
                        Email = usuarioDto.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password),
                        FechaRegistro = DateTime.UtcNow
                    };

                    await _usuarioRepository.AgregarAsync(usuario);

                    var token = GenerarToken(usuario);

                    await transaction.CommitAsync();
                    return token;
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        private string GenerarToken(Usuario usuario)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("id", usuario.Id.ToString()),
                new Claim("nombre", usuario.Nombre)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
