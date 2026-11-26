using RestApi.Datos;
using RestApi.Negocio;
using RestApi.Entidades;

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

        services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
        services.AddScoped<CategoriaServicio>();


        services.AddScoped<IMesaRepositorio, MesaRepositorio>();
        services.AddScoped<MesaServicio>();

        services.AddScoped<IPedidoRepositorio, PedidoRepositorio>();
        services.AddScoped<PedidoServicio>();

        services.AddScoped<IReporteRepositorio, ReporteRepositorio>();
        services.AddScoped<ReporteServicio>();

        return services;
    }
}