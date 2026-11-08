using RestApi.Datos;
using RestApi.Entidades;
using RestApi.Negocio;

var builder = WebApplication.CreateBuilder(args);

// 1. Inyectamos las dependencias desde nuestra nueva clase de extensión
builder.Services.AgregarServiciosRestaurante();

var app = builder.Build();

// ENDPOINTS DE PRODUCTOS

app.MapGet("/productos", (ProductoServicio servicio) =>
    servicio.ListarDisponibles());

app.MapGet("/productos/{id:int}", IResult (int id, ProductoServicio servicio) =>
{
    try
    {
        var producto = servicio.Buscar(id);
        if (producto is null)
            return Results.NotFound("No existe el producto " + id);
        return Results.Ok(producto);
    }
    catch (ArgumentException ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

// ENDPOINTS DE PLATOS
app.MapPlatoEndpoints();
app.Run();