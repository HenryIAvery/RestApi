using RestApi.Entidades;


public static class PlatoEndpoints
{
    public static void MapPlatoEndpoints(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/platos");

        grupo.MapGet("/", async (PlatoServicio s) =>
            Results.Ok(await s.ObtenerTodosAsync()));

        grupo.MapGet("/{id}", async (int id, PlatoServicio s) =>
        {
            var plato = await s.ObtenerPorIdAsync(id);
            return plato is null ? Results.NotFound(new { error = "Plato no encontrado." }) : Results.Ok(plato);
        });

        grupo.MapPost("/", async (Plato p, PlatoServicio s) =>
        {
            try
            {
                p.Id = await s.AgregarAsync(p);
                return Results.Created($"/platos/{p.Id}", p);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        grupo.MapPut("/{id}", async (int id, Plato p, PlatoServicio s) =>
        {
            if (id != p.Id) return Results.BadRequest(new { error = "El ID no coincide." });

            try
            {
                var actualizado = await s.ActualizarAsync(p);
                return actualizado ? Results.NoContent() : Results.NotFound(new { error = "Plato no encontrado." });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        });

        grupo.MapDelete("/{id}", async (int id, PlatoServicio s) =>
        {
            var eliminado = await s.EliminarAsync(id);
            return eliminado ? Results.NoContent() : Results.NotFound(new { error = "Plato no encontrado." });
        });
    }
}