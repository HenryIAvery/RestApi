---
name: revision-restapi
description: Revisa el código del proyecto RestApi antes de hacer un commit para verificar arquitectura en capas, Minimal APIs, reglas de negocio, borrado lógico, manejo de errores y configuración segura. Actívala cuando el usuario pida revisar el código antes de un commit o auditar cambios.
---

# Procedimiento de Revisión Pre-Commit (RestApi)

Cuando el usuario pida revisar el código antes del commit, analiza los archivos del proyecto en este orden y reporta cada incumplimiento marcado explícitamente con **✘**:

1. **Configuración y Conexión (`Program.cs`, `appsettings.json`):**
   - Verifica que las cadenas de conexión (`ConnectionStrings`) existan y no estén vacías ni hardcodeadas con contraseñas en el código fuente.
   - Comprueba que no haya registros duplicados de servicios o `DbContext` en `Program.cs`.

2. **Arquitectura en Capas (`Endpoints/`, `Negocio/`, `Datos/`, `Entidades/`):**
   - Verifica que los endpoints (`*Endpoints.cs` o `Program.cs`) no contengan lógica de negocio ni consultas directas a la base de datos; deben delegar siempre a la capa `Negocio`.
   - Comprueba que la capa `Negocio` valide los datos de entrada antes de llamar a `Datos`.

3. **Borrado Lógico y Reglas de Negocio:**
   - Verifica que las eliminaciones (`DELETE`) apliquen **borrado lógico** (cambiando un estado como `Activo = false` o `Disponible = false`) en lugar de borrar registros físicamente de la base de datos (`Remove` / `DELETE FROM`).

4. **Manejo de Errores y Códigos HTTP:**
   - Verifica que todos los endpoints capturen excepciones y devuelvan códigos HTTP semánticos (`200`, `201`, `204`, `400`, `404`, `409`) en lugar de dejar excepciones sin controlar (`500`).
