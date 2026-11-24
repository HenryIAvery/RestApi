using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestApi.Datos;
using RestApi.Endpoints;
using RestApi.Entidades;
using RestApi.Negocio;

var builder = WebApplication.CreateBuilder(args);

var cn = builder.Configuration
    .GetConnectionString("ConexionRestaurante");

// 1. Inyectamos las dependencias desde nuestra nueva clase de extensión
builder.Services.AgregarServiciosRestaurante();

builder.Services.AddDbContext<RestauranteContext>(
    o => o.UseMySql(cn, ServerVersion.AutoDetect(cn)));

// NUEVO: servicios de usuarios
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<UsuarioServicio>();

// NUEVO: los servicios del login
builder.Services.AddScoped<TokenServicio>();
builder.Services.AddScoped<AuthServicio>();

// Políticas de autorización
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("SoloAdmin",
        p => p.RequireRole(Roles.Administrador))
    .AddPolicy("Personal",
        p => p.RequireRole(Roles.Administrador,
                           Roles.Mesero, Roles.Cocina))
    .AddPolicy("Mesero",
        p => p.RequireRole(Roles.Mesero))
    .AddPolicy("Cocina",
        p => p.RequireRole(Roles.Cocina));

// Configuración de JWT
var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        // Deja "name" y "role" con el mismo nombre del token
        o.MapInboundClaims = false;

        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,      // vence a la hora exacta
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwt["Key"]!)),
            NameClaimType = "name",
            RoleClaimType = "role"
        };
    });

var app = builder.Build();

// SEGURIDAD (siempre antes de los endpoints)
app.UseAuthentication();   // ¿quién eres?   → 401
app.UseAuthorization();    // ¿qué puedes hacer? → 403

// ENDPOINTS DE LOGIN / AUTH
app.MapAuthEndpoints();

// ENDPOINTS DE PRODUCTOS
app.MapProductoEndpoints();

// ENDPOINTS DE PLATOS
app.MapPlatoEndpoints();

// ENDPOINTS DE CATEGORIAS
app.MapCategoriaEndpoints();

// ENDPOINTS DE MESAS
app.MapMesaEndpoints();

// ENDPOINTS DE USUARIOS
app.MapUsuarioEndpoints();

// ENDPOINTS DE PEDIDOS
app.MapPedidoEndpoints();

app.Run();