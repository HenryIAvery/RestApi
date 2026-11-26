using RestApi.Negocio;

namespace RestApi.Endpoints;

public static class ReporteEndpoints
{
    public static void MapReporteEndpoints(this IEndpointRouteBuilder app)
    {
        var g = app.MapGroup("/reportes")
                   .RequireAuthorization("SoloAdmin");

        // GET /reportes/dia?fecha=AAAA-MM-DD → SoloAdmin
        g.MapGet("/dia", (string? fecha, ReporteServicio s) =>
            Respuestas.Manejar(async () =>
                Results.Ok(await s.ObtenerDelDiaAsync(fecha))));
    }
}
