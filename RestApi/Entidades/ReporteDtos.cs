namespace RestApi.Entidades;

public record PlatoVendidoDto(
    string Plato,
    int Cantidad,
    decimal Total);

public record EstadoConteoDto(
    string Estado,
    int Cantidad);

public record ReporteDiaDto(
    DateOnly Fecha,
    int TotalPedidos,
    decimal TotalVendido,
    decimal TicketPromedio,
    List<PlatoVendidoDto> TopPlatos,
    List<EstadoConteoDto> PorEstado);
