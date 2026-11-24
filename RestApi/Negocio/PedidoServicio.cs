using RestApi.Datos;
using RestApi.Entidades;

namespace RestApi.Negocio;

public class PedidoServicio(
    IPedidoRepositorio pedidos,
    IMesaRepositorio mesas,
    IPlatoRepositorio platos)
{
    public Task<List<Pedido>> ListarAsync(string? estado = null) =>
        pedidos.ListarAsync(estado);

    public async Task<Pedido> ObtenerAsync(int id) =>
        await pedidos.ObtenerPorIdAsync(id)
        ?? throw new KeyNotFoundException($"No existe el pedido {id}.");

    public async Task<Pedido> CrearAsync(CrearPedidoRequest req)
    {
        if (req.Items is null || req.Items.Count == 0)
            throw new ArgumentException("El pedido debe tener al menos un plato.");

        var mesa = await mesas.ObtenerPorIdAsync(req.MesaId)
            ?? throw new KeyNotFoundException($"No existe la mesa {req.MesaId}.");

        var pedido = new Pedido
        {
            MesaId = mesa.Id,
            Fecha = DateTime.Now,
            Estado = "Pendiente"
        };

        foreach (var item in req.Items)
        {
            if (item.Cantidad <= 0)
                throw new ArgumentException("La cantidad de cada plato debe ser mayor a 0.");

            // El precio SIEMPRE se toma de la base de datos (y solo platos disponibles)
            var plato = await platos.ObtenerPorIdAsync(item.PlatoId)
                ?? throw new KeyNotFoundException(
                    $"El plato {item.PlatoId} no existe o no está disponible.");

            pedido.Detalles.Add(new DetallePedido
            {
                PlatoId = plato.Id,
                Cantidad = item.Cantidad,
                PrecioUnitario = plato.Precio
            });
        }

        await pedidos.AgregarAsync(pedido);
        return pedido;
    }

    public async Task<Pedido> AvanzarAsync(int id, string rol)
    {
        var pedido = await ObtenerAsync(id);

        // Flujo de estados y qué rol puede hacer cada transición:
        // Pendiente → EnPreparacion (Cocina)
        // EnPreparacion → Listo     (Cocina)
        // Listo → Entregado         (Mesero)
        var (siguiente, rolRequerido) = pedido.Estado switch
        {
            "Pendiente"     => ("EnPreparacion", Roles.Cocina),
            "EnPreparacion" => ("Listo",         Roles.Cocina),
            "Listo"         => ("Entregado",     Roles.Mesero),
            "Entregado"     => throw new InvalidOperationException(
                                   "El pedido ya fue entregado y no puede avanzar más."),
            _               => throw new InvalidOperationException(
                                   $"Estado desconocido: {pedido.Estado}.")
        };

        if (rol != rolRequerido && rol != Roles.Administrador)
            throw new UnauthorizedAccessException(
                $"Solo {rolRequerido} puede pasar el pedido a {siguiente}.");

        pedido.Estado = siguiente;
        await pedidos.GuardarCambiosAsync();
        return pedido;
    }
}
