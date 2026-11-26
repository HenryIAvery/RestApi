using Microsoft.EntityFrameworkCore;
using RestApi.Datos;
using RestApi.Entidades;

public class PlatoRepositorio(RestauranteContext db) : IPlatoRepositorio
{
    public Task<List<Plato>> ObtenerTodosAsync() =>
        (from p in db.Platos.AsNoTracking()
         join c in db.Categorias.AsNoTracking() on p.CategoriaId equals c.Id
         orderby p.Nombre
         select new Plato
         {
             Id = p.Id,
             Nombre = p.Nombre,
             Descripcion = p.Descripcion,
             Precio = p.Precio,
             Disponible = p.Disponible,
             CategoriaId = p.CategoriaId,
             Categoria = c.Nombre
         }).ToListAsync();

    public Task<Plato?> ObtenerPorIdAsync(int id) =>
        (from p in db.Platos
         join c in db.Categorias on p.CategoriaId equals c.Id
         where p.Id == id
         select new Plato
         {
             Id = p.Id,
             Nombre = p.Nombre,
             Descripcion = p.Descripcion,
             Precio = p.Precio,
             Disponible = p.Disponible,
             CategoriaId = p.CategoriaId,
             Categoria = c.Nombre
         }).FirstOrDefaultAsync();

    public Task<List<Plato>> ObtenerPorCategoriaAsync(int categoriaId) =>
        (from p in db.Platos.AsNoTracking()
         join c in db.Categorias.AsNoTracking() on p.CategoriaId equals c.Id
         where p.CategoriaId == categoriaId
         orderby p.Nombre
         select new Plato
         {
             Id = p.Id,
             Nombre = p.Nombre,
             Descripcion = p.Descripcion,
             Precio = p.Precio,
             Disponible = p.Disponible,
             CategoriaId = p.CategoriaId,
             Categoria = c.Nombre
         }).ToListAsync();

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