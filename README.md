# MVC_Contactos

Aplicación web ASP.NET Core MVC para administrar contactos (CRUD de personas).

## Objetivos

- Migrar un CRUD de ASP.NET MVC 5 a ASP.NET Core MVC.
- Reemplazar la lista en memoria por SQL Server con Entity Framework Core.

## Estado actual

- [x] Entorno validado y proyecto base creado.
- [x] Modelo, DbContext y migración.
- [x] CRUD de personas (`/CRUD_Persona`).
- [x] Módulo de facturación: páginas informativas (`/Principal`) y CRUD de empleados (`/TBLempleado`).

## Versiones y dependencias

| Tecnología | Versión |
|---|---|
| .NET SDK (instalado) | 11.0.100 |
| Framework objetivo | net10.0 |
| ASP.NET Core Runtime | 10.0.15 |
| C# | 14 (por defecto en .NET 10) |
| Entity Framework Core | 10.0.15 |
| dotnet-ef (herramienta global) | 10.0.15 |
| SQL Server | LocalDB, instancia `MSSQLLocalDB` |
| Git | 2.49.0 |
| Visual Studio Code | 1.147.0 |

### Paquetes NuGet

| Paquete | Versión |
|---|---|
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.15 |
| Microsoft.EntityFrameworkCore.Design | 10.0.15 |

## Requisitos previos

- Windows con PowerShell.
- .NET SDK 10 o superior (el proyecto apunta a `net10.0`; necesita el runtime 10.x).
- SQL Server LocalDB.
- Git.
- Visual Studio Code con las extensiones `ms-dotnettools.csdevkit` y `ms-dotnettools.csharp`.

## Instalación desde cero

```powershell
# 1. Verificar herramientas
dotnet --list-sdks
dotnet --list-runtimes
git --version
sqllocaldb info

# 2. Instalar herramienta de EF Core
dotnet tool install --global dotnet-ef --version 10.*
dotnet ef --version

# 3. Clonar el repositorio (evitar carpetas sincronizadas con OneDrive)
cd C:\Developments
git clone https://github.com/soypadierna/MVC_Contactos.git
cd MVC_Contactos

# 4. Restaurar, compilar y ejecutar
dotnet restore
dotnet build
dotnet run
```

## Configuración

Cadena de conexión en `appsettings.json` (clave `ContactosConnection`), apunta a LocalDB:

`Server=(localdb)\MSSQLLocalDB;Database=MVC_Contactos;Trusted_Connection=True;TrustServerCertificate=True`

Crear la base de datos y aplicar migraciones:

```powershell
dotnet ef database update
```

Abrir: `http://localhost:<puerto>/CRUD_Persona`

## Estructura del proyecto

| Ruta | Descripción |
|---|---|
| `Controllers/CRUD_PersonaController.cs` | CRUD de personas (EF Core) |
| `Controllers/TBLempleadoController.cs` | CRUD de empleados |
| `Controllers/PrincipalController.cs` | Páginas informativas |
| `Models/ModeloPersona.cs` | Entidad Persona con validaciones |
| `Models/TBLEMPLEADO.cs`, `Models/TBLROLES.cs` | Entidades de empleados y roles |
| `Data/ContactosDbContext.cs` | DbContext y datos iniciales |
| `Migrations/` | Migraciones de EF Core |
| `Views/CRUD_Persona/` | Vistas de personas |
| `Views/TBLempleado/`, `Views/Principal/` | Vistas de facturación |
| `Views/Shared/_LayoutModerno.cshtml` | Layout de personas (Bootstrap 3) |
| `Views/Shared/_LayoutFacturacion.cshtml` | Layout de facturación (Bootstrap 3) |
| `Program.cs` | Configuración y arranque |

## Base de datos

Tablas:
- `Personas`: `PersonaId` (PK), `Nombre`, `Telefono`, `Email`.
- `TBLEMPLEADO`: datos del empleado, `IdRolEmpleado` (FK a `TBLROLES`).
- `TBLROLES`: `IdRolEmpleado` (PK), `StrDescripcion`. Roles iniciales: Administrador, Vendedor, Cajero.

Migraciones: `InicialPersonas`, `AgregarEmpleados`.

## Historial de cambios

- Proyecto base creado en net10.0 con EF Core 10.0.15.
- Migración del CRUD de personas de MVC 5 a ASP.NET Core con SQL Server LocalDB.
- Migración del módulo de facturación: empleados, roles y páginas informativas.