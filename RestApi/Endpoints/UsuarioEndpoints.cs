using RestApi.Entidades;
using RestApi.Negocio;

namespace RestApi.Endpoints;
public static class UsuarioEndpoints
{
    public static void MapUsuarioEndpoints(
        this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/usuarios")
                   .RequireAuthorization("SoloAdmin");

        // GET /usuarios → 200 con la lista de UsuarioDto
        g.MapGet("", async (UsuarioServicio s) =>
            Results.Ok(await s.ListarAsync()));

        // POST /usuarios → 201, 400 o 409
        g.MapPost("", (RegistroUsuario r, UsuarioServicio s) =>
            Respuestas.Manejar(async () =>
            {
                var u = await s.RegistrarAsync(r);
                return Results.Created($"/usuarios/{u.Id}", u);
            }));

        // GET /usuarios → 200 · sin token 401 · otro rol 403
        g.MapGet("", async (UsuarioServicio s) =>
            Results.Ok(await s.ListarAsync()));
    }


}