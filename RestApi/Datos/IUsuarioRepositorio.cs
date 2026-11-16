namespace RestApi.Datos;
using RestApi.Entidades;

public interface IUsuarioRepositorio
{
    Task<List<Usuario>> ListarAsync();
    Task<Usuario?> ObtenerPorCorreoAsync(string correo);
    Task<bool> ExisteCorreoAsync(string correo);
    Task AgregarAsync(Usuario u);
}
