using RestApi.Datos;
using RestApi.Entidades;
using RestApi.Negocio;
using RestApi.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var cn = builder.Configuration
    .GetConnectionString("ConexionRestaurante");

// 1. Inyectamos las dependencias desde nuestra nueva clase de extensión
builder.Services.AgregarServiciosRestaurante();

builder.Services.AddDbContext<RestauranteContext>(
    o => o.UseMySql(cn, ServerVersion.AutoDetect(cn)));

var app = builder.Build();

// ENDPOINTS DE PRODUCTOS
app.MapProductoEndpoints();

// ENDPOINTS DE PLATOS
app.MapPlatoEndpoints();

//ENDPOINTS DE CATEGORIAS

app.MapCategoriaEndpoints();

//ENDPOINTS DE MESAS

app.MapMesaEndpoints();

app.Run();