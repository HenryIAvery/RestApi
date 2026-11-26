using RestApi.Entidades;

namespace RestApi.Datos;

public interface IReporteRepositorio
{
    Task<ReporteDiaDto> ObtenerDelDiaAsync(DateOnly fecha);
}
