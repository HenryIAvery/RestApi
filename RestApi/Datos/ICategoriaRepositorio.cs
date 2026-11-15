using RestApi.Entidades;

namespace RestApi.Datos;

public interface ICategoriaRepositorio
{
    Task<List<Categoria>> ObtenerTodasAsync();
    Task<Categoria> ObtenerPorIdAsync(int id);
    Task<int> AgregarAsync(Categoria c);
    Task<int> ActualizarAsync(Categoria c);
    Task<int> EliminarAsync(int id);

    // Método especial que usaste en tu servicio para validar antes de borrar
    Task<int> ContarPlatosAsync(int id);
}
