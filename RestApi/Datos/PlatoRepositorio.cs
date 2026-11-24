using Microsoft.EntityFrameworkCore;
using RestApi.Datos;
using RestApi.Entidades;

public class PlatoRepositorio(RestauranteContext db) : IPlatoRepositorio
{
    public Task<List<Plato>> ObtenerTodosAsync() =>
        db.Platos.AsNoTracking()
          .OrderBy(p => p.Nombre)
          .ToListAsync();

    public Task<Plato?> ObtenerPorIdAsync(int id) =>
        db.Platos.FirstOrDefaultAsync(p => p.Id == id);

    public Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId) =>
        db.Platos.AsNoTracking()
          .Where(p => p.CategoriaId == categoriaId)
          .OrderBy(p => p.Nombre)
          .ToListAsync();

    public async Task<int> AgregarAsync(Plato p)
    {
        p.Id = 0;
        db.Platos.Add(p);
        await db.SaveChangesAsync();
        return p.Id;
    }

    public async Task<bool> ActualizarAsync(Plato p)
    {
        var actual = await db.Platos.FirstOrDefaultAsync(x => x.Id == p.Id);
        if (actual is null) return false;

        actual.Nombre = p.Nombre;
        actual.Descripcion = p.Descripcion;
        actual.Precio = p.Precio;
        actual.Disponible = p.Disponible;
        actual.CategoriaId = p.CategoriaId;

        await db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var actual = await db.Platos.FirstOrDefaultAsync(x => x.Id == id);
        if (actual is null) return false;

        // Borrado lógico: Disponible = false
        actual.Disponible = false;
        await db.SaveChangesAsync();
        return true;
    }
}