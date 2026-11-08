using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using RestApi.Entidades;



public class PlatoRepositorio(IConfiguration config) : IPlatoRepositorio
{
    private readonly string _cn = "Server=localhost;Port=3306;Database=RestauranteITQ;Uid=root;Pwd=0998931186Hk!_P;";

    // PASO 2: LECTURA (Actualizado con 6 campos)
    public async Task<List<Plato>> ObtenerTodosAsync()
    {

        var lista = new List<Plato>();
        // Usamos la cadena literal en lugar de la variable global _cn
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync(); // Si la ejecución pasa esta línea, tu MySQL está perfecto.

        var cmd = new MySqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Disponible, CategoriaId FROM Platos ORDER BY Nombre", cn);

        await using var dr = await cmd.ExecuteReaderAsync();

        while (await dr.ReadAsync())
        {
            lista.Add(new Plato
            {
                Id = dr.GetInt32(0),
                Nombre = dr.GetString(1),
                Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                Precio = dr.GetDecimal(3),
                Disponible = dr.GetBoolean(4),
                CategoriaId = dr.GetInt32(5)
            });
        }
        return lista;
    }

    // PASO 3: ESCRITURA - INSERT (Actualizado con 6 campos)
    public async Task<int> AgregarAsync(Plato p)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        var cmd = new MySqlCommand(
            "INSERT INTO Platos (Nombre, Descripcion, Precio, Disponible, CategoriaId) VALUES (@nombre, @desc, @precio, @disponible, @cat); SELECT LAST_INSERT_ID();", cn);

        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@disponible", p.Disponible);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);

        return Convert.ToInt32(await cmd.ExecuteScalarAsync());
    }

    // PASO 3: ESCRITURA - UPDATE (Actualizado con 6 campos)
    public async Task<bool> ActualizarAsync(Plato p)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        var cmd = new MySqlCommand(
            "UPDATE Platos SET Nombre = @nombre, Descripcion = @desc, Precio = @precio, Disponible = @disponible, CategoriaId = @cat WHERE Id = @id", cn);

        cmd.Parameters.AddWithValue("@id", p.Id);
        cmd.Parameters.AddWithValue("@nombre", p.Nombre);
        cmd.Parameters.AddWithValue("@desc", p.Descripcion ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@precio", p.Precio);
        cmd.Parameters.AddWithValue("@disponible", p.Disponible);
        cmd.Parameters.AddWithValue("@cat", p.CategoriaId);

        var filasAfectadas = await cmd.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        // Cambiamos el comando DELETE por un UPDATE
        var cmd = new MySqlCommand("UPDATE Platos SET Disponible = 0 WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);

        var filasAfectadas = await cmd.ExecuteNonQueryAsync();
        return filasAfectadas > 0;
    }

    public async Task<Plato?> ObtenerPorIdAsync(int id)
    {
        await using var cn = new MySqlConnection(_cn);
        await cn.OpenAsync();

        var cmd = new MySqlCommand(
            "SELECT Id, Nombre, Descripcion, Precio, Disponible, CategoriaId FROM Platos WHERE Id = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);

        await using var dr = await cmd.ExecuteReaderAsync();
        if (await dr.ReadAsync())
        {
            return new Plato
            {
                Id = dr.GetInt32(0),
                Nombre = dr.GetString(1),
                Descripcion = dr.IsDBNull(2) ? null : dr.GetString(2),
                Precio = dr.GetDecimal(3),
                Disponible = dr.GetBoolean(4),
                CategoriaId = dr.GetInt32(5)
            };
        }
        return null; // Si no encontró el plato
    }
}