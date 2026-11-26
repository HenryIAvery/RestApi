using RestApi.Datos;
using RestApi.Entidades;

namespace RestApi.Negocio;

public class ReporteServicio(IReporteRepositorio repo)
{
    public Task<ReporteDiaDto> ObtenerDelDiaAsync(string? fechaTexto)
    {
        DateOnly fecha;

        if (string.IsNullOrWhiteSpace(fechaTexto))
        {
            fecha = DateOnly.FromDateTime(DateTime.Today);
        }
        else if (!DateOnly.TryParseExact(fechaTexto, "yyyy-MM-dd", out fecha))
        {
            throw new ArgumentException("Formato de fecha inválido. Use AAAA-MM-DD.");
        }

        return repo.ObtenerDelDiaAsync(fecha);
    }
}
