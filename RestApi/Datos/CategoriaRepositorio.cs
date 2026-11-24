namespace RestApi.Datos;

using Microsoft.EntityFrameworkCore;
using RestApi.Entidades;

public class CategoriaRepositorio(RestauranteContext db) : ICategoriaRepositorio
{
    public Task<List<Categoria>> ObtenerTodasAsync() =>
        db.Categorias.AsNoTracking()
          .OrderBy(c => c.Nombre)
          .ToListAsync();

    public Task<Categoria?> ObtenerPorIdAsync(int id) =>
        db.Categorias.FindAsync(id).AsTask();

    public async Task<int> AgregarAsync(Categoria c)
    {
        c.Id = 0;
        db.Categorias.Add(c);
        await db.SaveChangesAsync();
        return c.Id;
    }

    public async Task<int> ActualizarAsync(Categoria c)
    {
        var actual = await db.Categorias.FindAsync(c.Id);
        if (actual is null) return 0;
        actual.Nombre = c.Nombre;
        await db.SaveChangesAsync();
        return 1;
    }

    public async Task<int> EliminarAsync(int id)
    {
        var actual = await db.Categorias.FindAsync(id);
        if (actual is null) return 0;
        db.Categorias.Remove(actual);
        await db.SaveChangesAsync();
        return 1;
    }

    // ¿Cuántos platos usan esta categoría? (incluyendo no disponibles para evitar error de FK)
    public Task<int> ContarPlatosAsync(int id) =>
        db.Platos.IgnoreQueryFilters()
          .CountAsync(p => p.CategoriaId == id);
}
