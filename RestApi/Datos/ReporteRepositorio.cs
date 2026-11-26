using Microsoft.EntityFrameworkCore;
using RestApi.Entidades;

namespace RestApi.Datos;

public class ReporteRepositorio(RestauranteContext db) : IReporteRepositorio
{
    public async Task<ReporteDiaDto> ObtenerDelDiaAsync(DateOnly fecha)
    {
        var inicio = fecha.ToDateTime(TimeOnly.MinValue);
        var fin = inicio.AddDays(1);

        var pedidosDia = db.Pedidos
            .AsNoTracking()
            .Where(p => p.Fecha >= inicio && p.Fecha < fin);

        // 1. Cantidad de pedidos por estado con GroupBy en SQL
        var porEstado = await pedidosDia
            .GroupBy(p => p.Estado)
            .Select(g => new EstadoConteoDto(g.Key, g.Count()))
            .ToListAsync();

        var totalPedidos = porEstado.Sum(e => e.Cantidad);

        // 2. Total vendido y ticket promedio de pedidos Entregado
        var entregados = pedidosDia.Where(p => p.Estado == "Entregado");

        var cantEntregados = porEstado
            .Where(e => e.Estado == "Entregado")
            .Sum(e => e.Cantidad);

        var totalVendido = await entregados
            .SelectMany(p => p.Detalles)
            .SumAsync(d => (decimal?)d.Cantidad * d.PrecioUnitario) ?? 0m;

        var ticketPromedio = cantEntregados == 0
            ? 0m
            : Math.Round(totalVendido / cantEntregados, 2);

        // 3. Top 5 platos más vendidos (agrupando detalles por plato en SQL y ordenando por cantidad)
        var topPlatos = await entregados
            .SelectMany(p => p.Detalles)
            .Join(
                db.Platos.IgnoreQueryFilters(),
                d => d.PlatoId,
                pl => pl.Id,
                (d, pl) => new { pl.Id, pl.Nombre, d.Cantidad, d.PrecioUnitario })
            .GroupBy(x => new { x.Id, x.Nombre })
            .Select(g => new
            {
                Plato = g.Key.Nombre,
                Cantidad = g.Sum(x => x.Cantidad),
                Total = g.Sum(x => x.Cantidad * x.PrecioUnitario)
            })
            .OrderByDescending(x => x.Cantidad)
            .Take(5)
            .Select(x => new PlatoVendidoDto(x.Plato, x.Cantidad, x.Total))
            .ToListAsync();

        return new ReporteDiaDto(
            fecha,
            totalPedidos,
            totalVendido,
            ticketPromedio,
            topPlatos,
            porEstado);
    }
}
