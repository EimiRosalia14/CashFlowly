using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CashFlowly.Core.Application.DTOs.Usuario;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using CashFlowly.Infrastructure.Persistence.Repositories;
using CashFlowly.Infrastructure.Persistence.Contexts;
using System.Web;
using System.Text.RegularExpressions;

namespace CashFlowly.Infrastructure.Persistence.Services
{
    public class AuthService : IAuthService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;
        private readonly CashFlowlyDbContext _context;
        private readonly IEmailService _emailService;

        public AuthService(UsuarioRepository usuarioRepository, IConfiguration configuration, CashFlowlyDbContext context, IEmailService emailService)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
            _context = context;
            _emailService = emailService;
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

                    if (!EsContraseñaValida(usuarioDto.Password))
                    {
                        throw new Exception("La contraseña debe tener al menos 8 caracteres, una mayúscula y un número.");
                    }

                    var usuario = new Usuario
                    {
                        Nombre = usuarioDto.Nombre,
                        Email = usuarioDto.Email,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(usuarioDto.Password),
                        FechaRegistro = DateTime.UtcNow,
                        Confirmado = false,
                        TokenVerificacion = Guid.NewGuid().ToString()
                    };

                    await _usuarioRepository.AgregarAsync(usuario);

                    // Enviar el correo ANTES de confirmar la transacción
                    var backendUrl = _configuration["AppSettings:BackendUrl"];
                    var urlVerificacion = $"{backendUrl}/api/usuarios/confirmar?token={HttpUtility.UrlEncode(usuario.TokenVerificacion)}";
                    var mensaje = $"Hola {usuario.Nombre},\n\nGracias por registrarte en CashFlowly.\n\nPor favor verifica tu cuenta haciendo clic en el siguiente enlace: {urlVerificacion}\n\nSi no solicitaste esta cuenta, ignora este mensaje.\n\nSaludos,\nEl equipo de CashFlowly.";
                    await _emailService.EnviarCorreoAsync(usuario.Email, "Verificación de Cuenta", mensaje);

                    // Ahora sí confirmamos la transacción
                    await transaction.CommitAsync();

                    return "Usuario registrado exitosamente. Por favor revise su correo electrónico para activar la cuenta.";
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }


        public async Task<bool> ConfirmarCuentaAsync(string token)
        {
            try
            {
                var usuario = await _usuarioRepository.ObtenerPorTokenAsync(token);
                if (usuario == null)
                {
                    return false;
                }

                usuario.Confirmado = true;
                usuario.TokenVerificacion = null;

                await _usuarioRepository.ActualizarAsync(usuario);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al confirmar cuenta: {ex.InnerException?.Message ?? ex.Message}");
            }
        }


        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var usuario = await _usuarioRepository.ObtenerPorEmailAsync(loginDto.Email);

            if (usuario == null || usuario.Bloqueado || !usuario.Confirmado)
            {
                throw new UnauthorizedAccessException("Usuario no autorizado o cuenta no verificada.");
            }

            bool passwordValida = BCrypt.Net.BCrypt.Verify(loginDto.Password, usuario.PasswordHash);

            if (!passwordValida)
            {
                usuario.IntentosFallidos++;

<<<<<<< HEAD
                // 🚨 Bloqueo si excede intentos fallidos permitidos
                if (usuario.IntentosFallidos >= 10)
=======
                if (usuario.IntentosFallidos >= 5)
>>>>>>> 17dbf90331e1aa3658be59e37c2a60758441f42d
                {
                    usuario.Bloqueado = true;
                    await _usuarioRepository.ActualizarAsync(usuario);

                    await _emailService.EnviarCorreoAsync(usuario.Email, "Cuenta bloqueada",
                        "Tu cuenta ha sido bloqueada por múltiples intentos fallidos. Si no fuiste tú, contacta soporte.");

                    throw new UnauthorizedAccessException("Cuenta bloqueada por demasiados intentos fallidos.");
                }

                await _usuarioRepository.ActualizarAsync(usuario);
                throw new UnauthorizedAccessException("Usuario o contraseña incorrectos.");
            }

            usuario.IntentosFallidos = 0;
            await _usuarioRepository.ActualizarAsync(usuario);

            var token = GenerarToken(usuario);
            return token;
        }
        #region no implementado(desbloquear usuario)

        //public async Task<bool> DesbloquearCuentaAsync(string email)
        //{
        //    // Obtener el usuario por correo electrónico
        //    var usuario = await _usuarioRepository.ObtenerPorEmailAsync(email);

        //    // Verificar si el usuario existe
        //    if (usuario == null)
        //    {
        //        throw new Exception("El usuario no existe.");
        //    }

        //    // Desbloquear la cuenta y reiniciar intentos fallidos
        //    usuario.Bloqueado = false;
        //    usuario.IntentosFallidos = 0;

        //    // Generar una nueva clave
        //    usuario.Clave = GenerarClave();

        //    // Actualizar el usuario en la base de datos
        //    await _usuarioRepository.ActualizarAsync(usuario);

        //    // Enviar notificación por correo
        //    var subject = "Tu cuenta ha sido desbloqueada";
        //    var body = $"Hola {usuario.Nombre},<br/><br/>" +
        //                "Tu cuenta ha sido desbloqueada exitosamente. " +
        //                "Ahora puedes iniciar sesión nuevamente.<br/><br/>" +
        //                $"Tu nueva clave es: {usuario.PasswordHash}"; // Enviar la nueva clave en el correo

        //    // Asumimos que tienes un servicio de correo inyectado
        //    await _emailService.EnviarCorreoAsync(usuario.Email, subject, body);

        //    return true; // Indicar que la operación fue exitosa
        //}
        #endregion


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

        private bool EsContraseñaValida(string password)
        {
            return Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*\d)[A-Za-z\d]{8,}$");
        }
    }
}
