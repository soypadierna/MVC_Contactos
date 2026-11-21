# MVC_Contactos

Aplicación web ASP.NET Core MVC con tres módulos: contactos (personas), empleados y clientes.

## Objetivos

- Migrar proyectos de ASP.NET MVC 5 (.NET Framework) a ASP.NET Core MVC (.NET 10).
- Reemplazar la lista en memoria por SQL Server con Entity Framework Core.
- Mantener un módulo con acceso directo a MySQL mediante ADO.NET.

## Funcionalidades

| Módulo | Ruta | Acceso a datos | Base de datos |
|---|---|---|---|
| CRUD de personas | `/CRUD_Persona` | EF Core | SQL Server LocalDB |
| Empleados y roles | `/TBLempleado` | EF Core | SQL Server LocalDB |
| Páginas informativas | `/Principal` | No aplica | No aplica |
| CRUD de clientes | `/Clientes` | ADO.NET | MySQL remoto |

## Arquitectura

Patrón MVC. Los controladores de Personas y Empleados reciben el `ContactosDbContext` por inyección de dependencias. El módulo de Clientes usa la clase `ClienteDatos`, que ejecuta SQL con `MySqlConnection`.

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
| MySQL | Servidor remoto (base `dbfacturas`) |
| Git | 2.49.0 |
| Visual Studio Code | 1.147.0 |

### Paquetes NuGet

| Paquete | Versión |
|---|---|
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.15 |
| Microsoft.EntityFrameworkCore.Design | 10.0.15 |
| MySql.Data | 26.10.0 |

## Requisitos previos

- Windows con PowerShell.
- .NET SDK 10 o superior (el proyecto apunta a `net10.0`; necesita el runtime 10.x).
- SQL Server LocalDB.
- Git.
- Visual Studio Code con las extensiones `ms-dotnettools.csdevkit` y `ms-dotnettools.csharp`.
- Acceso de red al servidor MySQL del módulo de Clientes.

## Instalación desde cero

```powershell
# 1. Verificar herramientas
dotnet --list-sdks
dotnet --list-runtimes
git --version
sqllocaldb info

# 2. Instalar la herramienta de EF Core
dotnet tool install --global dotnet-ef --version 10.*
dotnet ef --version

# 3. Clonar el repositorio (evitar carpetas sincronizadas con OneDrive)
cd C:\Developments
git clone https://github.com/soypadierna/MVC_Contactos.git
cd MVC_Contactos

# 4. Restaurar paquetes y compilar
dotnet restore
dotnet build

# 5. Crear la base de datos y aplicar migraciones
dotnet ef database update
```

## Configuración

Las cadenas de conexión están en `appsettings.json`, sección `ConnectionStrings`:

| Clave | Uso |
|---|---|
| `ContactosConnection` | SQL Server LocalDB (Personas y Empleados) |
| `MiConexionMySQL` | MySQL remoto (Clientes) |

Cadena de LocalDB:

```
Server=(localdb)\MSSQLLocalDB;Database=MVC_Contactos;Trusted_Connection=True;TrustServerCertificate=True
```

Cadena de MySQL (reemplazar con credenciales propias, no subirlas a un repositorio público):

```
server=<servidor>;port=3306;database=dbfacturas;uid=<usuario>;pwd=<clave>;
```

## Ejecución

```powershell
dotnet restore
dotnet build
dotnet run
```

Abrir en el navegador la URL que muestre la consola (`http://localhost:<puerto>`):

- `/CRUD_Persona`
- `/Principal`
- `/TBLempleado`
- `/Clientes`

Para detener la aplicación: `Ctrl+C`. Si `dotnet build` falla porque el `.exe` está bloqueado, la aplicación sigue corriendo y hay que detenerla primero.

## Estructura del proyecto

| Ruta | Descripción |
|---|---|
| `Controllers/CRUD_PersonaController.cs` | CRUD de personas (EF Core) |
| `Controllers/TBLempleadoController.cs` | CRUD de empleados (EF Core) |
| `Controllers/PrincipalController.cs` | Páginas informativas |
| `Controllers/ClientesController.cs` | CRUD de clientes |
| `Models/ModeloPersona.cs` | Entidad Persona con validaciones |
| `Models/TBLEMPLEADO.cs`, `Models/TBLROLES.cs` | Entidades de empleados y roles |
| `Models/Cliente.cs` | Entidad Cliente |
| `Models/ClienteDatos.cs` | Acceso a datos ADO.NET para clientes |
| `Data/ContactosDbContext.cs` | DbContext y datos iniciales |
| `Migrations/` | Migraciones de EF Core |
| `Views/CRUD_Persona/` | Vistas de personas |
| `Views/TBLempleado/`, `Views/Principal/` | Vistas de facturación |
| `Views/Clientes/` | Vistas Index, Create, Edit, Delete y `_Campos` |
| `Views/Shared/_LayoutModerno.cshtml` | Layout de personas (Bootstrap 3) |
| `Views/Shared/_LayoutFacturacion.cshtml` | Layout de facturación (Bootstrap 3) |
| `Program.cs` | Configuración y arranque |
| `appsettings.json` | Cadenas de conexión y configuración |

## Base de datos

### SQL Server LocalDB (EF Core)

- `Personas`: `PersonaId` (PK), `Nombre`, `Telefono`, `Email`. Se siembran 4 registros iniciales.
- `TBLEMPLEADO`: datos del empleado, `IdRolEmpleado` (FK a `TBLROLES`).
- `TBLROLES`: `IdRolEmpleado` (PK), `StrDescripcion`. Roles iniciales: Administrador, Vendedor, Cajero.

Migraciones: `InicialPersonas`, `AgregarEmpleados`.

### MySQL remoto (ADO.NET)

- `TBLCLIENTES` (base `dbfacturas`): `IdCliente` (PK), `StrNombre`, `NumDocumento`, `StrDireccion`, `StrTelefono`, `StrEmail`, `DtmFechaModifica`, `StrUsuarioModifica`.
- No usa migraciones de EF Core. La tabla debe existir en el servidor.

## Historial de cambios

- Proyecto base creado en net10.0 con EF Core 10.0.15.
- Migración del CRUD de personas de MVC 5 a ASP.NET Core con SQL Server LocalDB.
- Migración del módulo de facturación: empleados, roles y páginas informativas.
- Módulo de Clientes con ADO.NET y MySQL.