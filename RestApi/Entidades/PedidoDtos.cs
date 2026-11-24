namespace RestApi.Entidades;

// Lo que ENTRA al crear un pedido
public record ItemPedidoRequest(int PlatoId, int Cantidad);

public record CrearPedidoRequest(int MesaId, List<ItemPedidoRequest> Items);
