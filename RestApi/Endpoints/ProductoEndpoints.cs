namespace RestApi.Endpoints;

using RestApi.Negocio;

public static class ProductoEndpoints
{
    public static void MapProductoEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/productos");

        grupo.MapGet("/", (ProductoServicio servicio) =>
            servicio.ListarDisponibles());

        grupo.MapGet("/{id:int}", IResult (int id, ProductoServicio servicio) =>
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
    }
}
