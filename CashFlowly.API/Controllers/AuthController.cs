﻿using CashFlowly.Core.Application.DTOs.Usuario;
using CashFlowly.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CashFlowly.API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("registrar")]
        public async Task<IActionResult> RegistrarUsuario([FromBody] UsuarioRegistroDto usuarioDto)
        {
            try
            {
                var token = await _authService.RegistrarUsuarioAsync(usuarioDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var token = await _authService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error en el servidor.", Error = ex.Message });
            }
        }
        /*
        [HttpGet("confirmar")]
        public async Task<IActionResult> ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                bool confirmado = await _authService.ConfirmarCuentaAsync(token);
                if (!confirmado)
                {
                    return BadRequest(new { mensaje = "Token inválido o cuenta ya confirmada." });
                }

                return Ok(new { mensaje = "Cuenta confirmada exitosamente. Ahora puede iniciar sesión." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }*/

        [HttpGet("confirmar")]
        public async Task<IActionResult> ConfirmarCuenta([FromQuery] string token)
        {
            try
            {
                bool confirmado = await _authService.ConfirmarCuentaAsync(token);
                if (!confirmado)
                {
                    return Redirect("https://localhost:7248/api/usuarios/error");
                }

                return Redirect("https://localhost:7248/api/usuarios/exito");
            }
            catch (Exception ex)
            {
                return Redirect("https://localhost:7248/api/usuarios/error");
            }
        }


        [HttpGet("exito")]
        public IActionResult Exito()
        {
            return Content("<html><body style='font-family:Arial; text-align:center; margin-top:50px;'>" +
                           "<h1 style='color:green;'>Cuenta confirmada exitosamente</h1>" +
                           "<p>Ahora puedes iniciar sesion.</p></body></html>", "text/html");
        }

        [HttpGet("error")]
        public IActionResult Error()
        {
            return Content("<html><body style='font-family:Arial; text-align:center; margin-top:50px;'>" +
                           "<h1 style='color:red;'>Error al confirmar cuenta</h1>" +
                           "<p>El token es invalido o la cuenta ya fue confirmada.</p></body></html>", "text/html");
        }
    }
}
