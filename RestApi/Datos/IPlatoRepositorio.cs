using RestApi.Entidades;

public interface IPlatoRepositorio
{
    Task<List<Plato>> ObtenerTodosAsync();
    Task<Plato?> ObtenerPorIdAsync(int id);
    Task<int> AgregarAsync(Plato p);
    Task<bool> ActualizarAsync(Plato p);
    Task<bool> EliminarAsync(int id);
}