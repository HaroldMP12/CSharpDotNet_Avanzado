# C# Avanzado

Este repositorio contiene el material y los proyectos desarrollados durante el curso de Programación en C# Avanzado.

# API de Gestión de Tareas

Esta API permite gestionar tareas, permitiendo crear, leer, actualizar y eliminar tareas (CRUD). Está desarrollada en C# .NET y utiliza SQL Server como base de datos.

## Configuración

### Requisitos previos
- .NET SDK instalado
- SQL Server instalado y configurado
- Visual Studio o VS Code (opcional pero recomendado)

### Instalación
1. Clona el repositorio:
   ```sh
   git clone <URL_DEL_REPOSITORIO>
   cd <NOMBRE_DEL_PROYECTO>
   ```
2. Configura la conexión a la base de datos en `appsettings.json`:
   ```
   "ConnectionStrings": {
     "DefaultConnection": "Server=TU_SERVIDOR;Database=TareasDB;User Id=TU_USUARIO;Password=TU_CONTRASEÑA;"
   }
   ```
3. Aplica las migraciones a la base de datos:
   ```sh
   dotnet ef database update
   ```
4. Ejecuta la API:
   ```sh
   dotnet run
