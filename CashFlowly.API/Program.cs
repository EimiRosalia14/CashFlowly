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
using CashFlowly.Core.Application.Services.Metas;

var builder = WebApplication.CreateBuilder(args);

var connection = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development"
                ? builder.Configuration.GetConnectionString("DefaultConnection") 
                : Environment.GetEnvironmentVariable("PRODUCTION_DB_CONNECTION");

builder.Services.AddDbContext<CashFlowlyDbContext>(options =>
    options.UseSqlServer(connection));

// Registrar HttpContextAccessor
builder.Services.AddHttpContextAccessor();


// AI
builder.Services.AddHttpClient("OpenAIClient", client =>
{
    client.BaseAddress = new Uri("https://api.openai.com/v1/");
});

builder.Services.AddScoped<IRecommendationService, RecommendationService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IGastoService, GastoService>();
builder.Services.AddScoped<ICuentasService, CuentasService>();
builder.Services.AddScoped<IIngresosService, IngresosService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IMetaService, MetaService>();
builder.Services.AddScoped<IMetasRecommendationService, MetasRecommendationService>();

// InyecciÃ³n de Dependencias - Repositorios
builder.Services.AddScoped<IGastosRepository, GastosRepository>();
builder.Services.AddScoped<ICuentasRepository, CuentasRepository>();
builder.Services.AddScoped<IIngresosRepository, IngresosRepository>();
builder.Services.AddScoped<ICategoriaRepository<CategoriaIngreso>, CategoriaRepository<CategoriaIngreso>>();
builder.Services.AddScoped<ICategoriaRepository<CategoriaGasto>, CategoriaRepository<CategoriaGasto>>();
builder.Services.AddScoped<ICategoriaIngresoPersonalizadaRepository, CategoriaIngresoPersonalizadaRepository>();
builder.Services.AddScoped<ICategoriaGastoPersonalizadaRepository, CategoriaGastoPersonalizadaRepository>();
builder.Services.AddScoped<IMetaRepository, MetaRepository>();


// InyecciÃ³n de AutoMapper
builder.Services.AddAutoMapper(typeof(DefaultProfile));

// ConfiguraciÃ³n de AutenticaciÃ³n y AutorizaciÃ³n
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
/*
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
*/
builder.Services.AddAuthorization();
builder.Services.AddControllers();

// ConfiguraciÃ³n de Swagger con AutenticaciÃ³n JWT
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

app.UseCors("AllowLocalhost3000");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
