using Microsoft.EntityFrameworkCore;
using RestApi.Entidades;

namespace RestApi.Datos;

public class PedidoRepositorio(RestauranteContext db) : IPedidoRepositorio
{
    public async Task<List<Pedido>> ListarAsync(string? estado = null)
    {
        var q = db.Pedidos
            .AsNoTracking()
            .Include(p => p.Mesa)
            .Include(p => p.Detalles)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(estado))
            q = q.Where(p => p.Estado == estado);

        return await q.OrderByDescending(p => p.Fecha).ToListAsync();
    }

    public Task<Pedido?> ObtenerPorIdAsync(int id) =>
        db.Pedidos
          .Include(p => p.Mesa)
          .Include(p => p.Detalles)
          .FirstOrDefaultAsync(p => p.Id == id);

    public async Task<int> AgregarAsync(Pedido pedido)
    {
        db.Pedidos.Add(pedido);
        await db.SaveChangesAsync();
        return pedido.Id;
    }

    public Task GuardarCambiosAsync() =>
        db.SaveChangesAsync();
}
