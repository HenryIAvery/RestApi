---
name: mejoras-restapi-dotnet
description: Propone e implementa mejoras profesionales en Minimal APIs de C# .NET (DTOs, IConfiguration, paginación, filtros y manejo global de errores).
---

# Mejoras de Producción para REST API en .NET

Al recibir la instrucción de mejorar o refactorizar la API, aplica o propón estas mejoras manteniendo la arquitectura N-capas:

1. **Separación de Entidades y DTOs:** Evita exponer la entidad de base de datos directamente en `POST` y `PUT` creando `CrearDto`, `ActualizarDto` y `RespuestaDto`.
2. **Configuración Limpia (`appsettings.json`):** Sustituye cadenas hardcodeadas por `IConfiguration`.
3. **Filtros y Paginación:** Agrega parámetros opcionales `[FromQuery]` (`page`, `pageSize`) en los listados `GET`.
4. **Documentación OpenAPI / Swagger:** Configura `.WithSummary()` y `.Produces<T>(statusCode)` en los endpoints.
