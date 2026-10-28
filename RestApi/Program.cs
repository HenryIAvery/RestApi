var builder = WebApplication.CreateBuilder(args);


var app = builder.Build();

app.MapGet("/saludo", () => "hola mundo");
app.MapGet("/saludos/{nombre}", (string nombre) =>
    $"Hola, {nombre}. Bienvenido a desarrollo web 2");

app.MapGet("/operaciones", (int a, int b) => new
{
    a,
    b,
    resultadoSuma = a + b,
    resultadoResta = a - b,
    resultadoMultiplicacion = a * b,
    // Si b es 0 devuelve null (o 0), si no, hace la división con decimales
    resultadoDivision = b == 0 ? (double?)null : (double)a / b
});


app.MapGet("/edad/{anio}", (int anio) =>
{
    var anioActual = DateTime.UtcNow.Year;

    // Controlamos que no pongan un año en el futuro o imposible
    if (anio > anioActual || anio < 1900)
    {
        return Results.BadRequest(new { error = "Año inválido" });
    }

    var edad = anioActual - anio;

    return Results.Ok(new { anio, edad, mayorEdad = edad >= 18 });
});

app.Run();
