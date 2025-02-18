﻿using CashFlowly.Core.Application.DTOs.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowly.Core.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<string> RegistrarUsuarioAsync(UsuarioRegistroDto usuarioDto);
    }
}
