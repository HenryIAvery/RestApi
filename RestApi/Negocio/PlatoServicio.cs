using RestApi.Entidades;

public class PlatoServicio(IPlatoRepositorio repo)
{
    public async Task<List<Plato>> ObtenerTodosAsync()
    {

        return await repo.ObtenerTodosAsync();
    }

    public async Task<int> AgregarAsync(Plato p)
    {
        // Ejemplo de lógica de negocio antes de MySQL
        if (p.Precio <= 0)
        {
            throw new ArgumentException("El precio del plato debe ser mayor a 0.");
        }

        return await repo.AgregarAsync(p);
    }

    public async Task<bool> ActualizarAsync(Plato p)
    {
        // Lógica de negocio: Validar que el precio sea correcto
        if (p.Precio <= 0)
        {
            throw new ArgumentException("El precio del plato debe ser mayor a 0.");
        }

        // Llama al método que ya creaste en el repositorio
        return await repo.ActualizarAsync(p);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        // Aquí podrías validar reglas de negocio en el futuro
        return await repo.EliminarAsync(id);
    }

    public async Task<Plato?> ObtenerPorIdAsync(int id)
    {
        return await repo.ObtenerPorIdAsync(id);
    }
    public async Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId)
    {
        return await repo.ObtenerPorCategoriaAsync(categoriaId);
    }
}
