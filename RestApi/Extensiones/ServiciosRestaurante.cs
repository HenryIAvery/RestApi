using RestApi.Datos;
using RestApi.Negocio;

public static class ServiciosRestaurante
{
    public static IServiceCollection AgregarServiciosRestaurante(this IServiceCollection services)
    {
        // Registramos lo de Producto que ya tenías
        services.AddScoped<ProductoRepositorio>();
        services.AddScoped<ProductoServicio>();

        // Registramos lo nuevo de Platos usando la Interfaz
        services.AddScoped<IPlatoRepositorio, PlatoRepositorio>();
        services.AddScoped<PlatoServicio>();

        return services;
    }
}