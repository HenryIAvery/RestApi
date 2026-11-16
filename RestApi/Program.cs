using Microsoft.EntityFrameworkCore;
using RestApi.Datos;
using RestApi.Endpoints;
using RestApi.Negocio;

var builder = WebApplication.CreateBuilder(args);

var cn = builder.Configuration
    .GetConnectionString("ConexionRestaurante");

// 1. Inyectamos las dependencias desde nuestra nueva clase de extensión
builder.Services.AgregarServiciosRestaurante();

builder.Services.AddDbContext<RestauranteContext>(
    o => o.UseMySql(cn, ServerVersion.AutoDetect(cn)));

builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<UsuarioServicio>();

var app = builder.Build();

// ENDPOINTS DE PRODUCTOS
app.MapProductoEndpoints();

// ENDPOINTS DE PLATOS
app.MapPlatoEndpoints();

//ENDPOINTS DE CATEGORIAS

app.MapCategoriaEndpoints();

//ENDPOINTS DE MESAS

app.MapMesaEndpoints();

//ENDPOINTS DE USUARIOS

app.MapUsuarioEndpoints();

app.Run();