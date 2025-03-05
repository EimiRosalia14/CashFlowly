using CashFlowly.Core.Application.Interfaces.Repositories;
using CashFlowly.Core.Application.Interfaces.Services;
using CashFlowly.Infrastructure.Persistence.Repositories;
using CashFlowly.Infrastructure.Persistence.Services;
using CashFlowly.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CashFlowly.Core.Application.Services;
using CashFlowly.Core.Domain.Entities;
using Microsoft.AspNetCore.Authentication.Cookies;
using CashFlowly.Core.Application.Services.Gasto;
using CashFlowly.Core.Application.Services.Cuentas;
using CashFlowly.Core.Application.Services.Ingresos;
using CashFlowly.Core.Application.Mappings;
using CashFlowly.Core.Application.Interfaces.Repositories.CashFlowly.Core.Application.Interfaces.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la Base de Datos
builder.Services.AddDbContext<CashFlowlyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Inyección de Dependencias - Servicios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGastoService, GastoService>();
builder.Services.AddScoped<ICuentasService, CuentasService>();
builder.Services.AddScoped<IIngresosService, IngresosService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();  

// Inyección de Dependencias - Repositorios
builder.Services.AddScoped<IGastosRepository, GastosRepository>();
builder.Services.AddScoped<ICuentasRepository, CuentasRepository>();
builder.Services.AddScoped<IIngresosRepository, IngresosRepository>();
builder.Services.AddScoped<ICategoriaRepository<CategoriaIngreso>, CategoriaRepository<CategoriaIngreso>>();
builder.Services.AddScoped<ICategoriaRepository<CategoriaGasto>, CategoriaRepository<CategoriaGasto>>();
builder.Services.AddScoped<ICategoriaIngresoPersonalizadaRepository, CategoriaIngresoPersonalizadaRepository>();
builder.Services.AddScoped<ICategoriaGastoPersonalizadaRepository, CategoriaGastoPersonalizadaRepository>();

// Inyección de AutoMapper
builder.Services.AddAutoMapper(typeof(DefaultProfile));

// Configuración de Autenticación y Autorización
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.LoginPath = "/api/usuarios/login"; // Ruta de login
});

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Configuración de Swagger con Autenticación JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CashFlowly.API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token en el formato: Bearer {token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// Habilitar Swagger solo en Desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();