using System.Security.Claims;
using RestApi.Entidades;
using RestApi.Negocio;

namespace RestApi.Endpoints;

public static class PedidoEndpoints
{
    public static void MapPedidoEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/pedidos");

        // GET /pedidos?estado=Pendiente → Personal (Admin, Mesero, Cocina)
        g.MapGet("", async (string? estado, PedidoServicio s) =>
            Results.Ok(await s.ListarAsync(estado)))
         .RequireAuthorization("Personal");

        // GET /pedidos/{id} → Personal
        g.MapGet("/{id:int}", (int id, PedidoServicio s) =>
            Respuestas.Manejar(async () =>
                Results.Ok(await s.ObtenerAsync(id))))
         .RequireAuthorization("Personal");

        // POST /pedidos → Público (cliente desde el QR) o Mesero; toma el precio de la BD
        g.MapPost("", (CrearPedidoRequest req, PedidoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                var p = await s.CrearAsync(req);
                return Results.Created($"/pedidos/{p.Id}", p);
            }));

        // PATCH /pedidos/{id}/avanzar → Personal (valida rol según el estado actual)
        g.MapPatch("/{id:int}/avanzar", (int id, ClaimsPrincipal user, PedidoServicio s) =>
            Respuestas.Manejar(async () =>
            {
                var rol = user.FindFirstValue("role") ?? "";
                var p = await s.AvanzarAsync(id, rol);
                return Results.Ok(p);
            }))
         .RequireAuthorization("Personal");
    }
}
