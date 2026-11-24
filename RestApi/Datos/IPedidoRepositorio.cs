using RestApi.Entidades;

namespace RestApi.Datos;

public interface IPedidoRepositorio
{
    Task<List<Pedido>> ListarAsync(string? estado = null);
    Task<Pedido?> ObtenerPorIdAsync(int id);
    Task<int> AgregarAsync(Pedido pedido);
    Task GuardarCambiosAsync();
}
