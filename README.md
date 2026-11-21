# MVC_Contactos

Sistema de facturación en ASP.NET Core MVC (.NET 10) con gestión de empleados, clientes, productos, categorías, facturas e informes.

## Objetivos

- Migrar un proyecto de ASP.NET MVC 5 (.NET Framework) a ASP.NET Core MVC (.NET 10).
- Usar SQL Server con Entity Framework Core para empleados, productos, categorías y facturas.
- Mantener el módulo de Clientes con acceso directo a MySQL mediante ADO.NET.

## Módulos

| Módulo | Ruta | Acceso a datos | Base de datos |
|---|---|---|---|
| Clientes | `/Clientes` | ADO.NET | MySQL remoto |
| Productos | `/Productos` | EF Core | SQL Server LocalDB |
| Categorías de producto | `/Categorias` | EF Core | SQL Server LocalDB |
| Facturas | `/Facturas` | EF Core | SQL Server LocalDB (clientes desde MySQL) |
| Informes | `/Informes` | EF Core | SQL Server LocalDB (clientes desde MySQL) |
| Empleados | `/TBLempleado` | EF Core | SQL Server LocalDB |
| Páginas informativas | `/Principal` | No aplica | No aplica |

Los ítems del menú Roles y Seguridad quedan marcados "en construcción".

## Funcionalidades

- **Empleados**: alta, edición, retiro y reactivación (no se borran). Estado calculado: activo, retiro programado (fecha futura) o retirado. Búsqueda por nombre, documento o correo e interruptor para mostrar retirados.
- **Clientes**: alta, edición y eliminación con modal de confirmación. Documento único solo si viene informado. No se elimina un cliente con facturas.
- **Categorías**: CRUD con descripción única. No se elimina una con productos.
- **Productos**: CRUD con código único, precios de compra y venta mayores a 0, categoría y stock entero.
- **Facturas**:
  - Se elige cliente, emisor y líneas de producto con cantidad. El precio se toma del precio de venta vigente.
  - Cálculo: subtotal, descuento (% de 0 a 100), IVA (19 % por defecto, configurable) y total, con 2 decimales.
  - La creación es transaccional, con bloqueo de productos (`UPDLOCK`) y descuento de stock.
  - Estados: Pendiente (1), Pagada (2), Anulada (3), Vencida (4). Transiciones: 1 → 2, 3 o 4; 2 → 3; 4 → 2 o 3; Anulada es final. Anular repone el stock.
  - No existe el borrado de facturas.
- **Informes** (en vivo, no se guardan): Ventas, Inventario, Financiero, Empleados y Clientes. Rango máximo de 366 días, fechas como días locales convertidas a UTC y exportación a CSV con `;` y BOM.

## Arquitectura

Patrón MVC. Los controladores reciben el `ContactosDbContext` por inyección de dependencias. Los informes usan `InformesService`. El módulo de Clientes usa la clase `ClienteDatos`, que ejecuta SQL con `MySqlConnection`. Las fechas de facturación se guardan en UTC.

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
| Bootstrap | 3.4.1 (CDN) |
| jQuery | 3.5.1 (CDN) |

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

# 5. Crear la base de datos y aplicar migraciones (incluye datos de ejemplo)
dotnet ef database update
```

## Configuración

Las cadenas de conexión y el IVA están en `appsettings.json`:

| Clave | Uso |
|---|---|
| `ConnectionStrings:ContactosConnection` | SQL Server LocalDB |
| `ConnectionStrings:MiConexionMySQL` | MySQL remoto (Clientes) |
| `Facturacion:IvaPorcentaje` | IVA en porcentaje (19 por defecto) |

Cadena de LocalDB:

```
Server=(localdb)\MSSQLLocalDB;Database=MVC_Contactos;Trusted_Connection=True;TrustServerCertificate=True
```

Cadena de MySQL (reemplazar con credenciales propias; no subir credenciales reales a un repositorio público):

```
server=<servidor>;port=3306;database=dbfacturas;uid=<usuario>;pwd=<clave>;
```

## Ejecución

```powershell
dotnet restore
dotnet build
dotnet run
```

Abrir en el navegador la URL que muestre la consola (`http://localhost:<puerto>`). La ruta por defecto es `/Principal/Index`.

Para detener la aplicación: `Ctrl+C`. Si `dotnet build` falla porque el `.exe` está bloqueado, la aplicación sigue corriendo y hay que detenerla primero.

## Estructura del proyecto

| Ruta | Descripción |
|---|---|
| `Controllers/TBLempleadoController.cs` | Empleados: listado, alta, edición, retiro y reactivación |
| `Controllers/ClientesController.cs` | CRUD de clientes |
| `Controllers/ProductosController.cs` | CRUD de productos |
| `Controllers/CategoriasController.cs` | CRUD de categorías |
| `Controllers/FacturasController.cs` | Facturas: listado, creación, detalle y cambio de estado |
| `Controllers/InformesController.cs` | Informes y exportación CSV |
| `Controllers/PrincipalController.cs` | Páginas informativas y "en construcción" |
| `Services/InformesService.cs` | Generación de informes en vivo y CSV |
| `Models/` | Entidades (`TBLEMPLEADO`, `TBLROLES`, `TBLCATEGORIAS`, `TBLPRODUCTOS`, `TBLFACTURAS`, `TBLFACTURADETALLE`), `Cliente`, `ClienteDatos` y modelos de vista |
| `Data/ContactosDbContext.cs` | DbContext, relaciones y datos iniciales |
| `Migrations/` | Migraciones de EF Core |
| `Views/` | Vistas por módulo |
| `Views/Shared/_LayoutFacturacion.cshtml` | Layout único (Bootstrap 3, responsive) |
| `Program.cs` | Configuración y arranque |
| `appsettings.json` | Cadenas de conexión y configuración |

## Base de datos

### SQL Server LocalDB (EF Core)

- `TBLROLES`: `IdRolEmpleado` (PK), `StrDescripcion`. Roles iniciales: Administrador, Vendedor, Cajero.
- `TBLEMPLEADO`: datos del empleado, `IdRolEmpleado` (FK), `DtmIngreso`, `DtmRetiro`.
- `TBLCATEGORIAS`: `IdCategoria` (PK), `StrDescripcion` (única).
- `TBLPRODUCTOS`: nombre, código (único), precios de compra y venta, `IdCategoria` (FK), detalle, stock.
- `TBLFACTURAS`: `IdCliente` (referencia lógica al cliente en MySQL), `IdEmpleado` (FK), fecha UTC, estado, subtotal, descuento, IVA y total.
- `TBLFACTURADETALLE`: `IdFactura` (FK), `IdProducto` (FK), cantidad, precio unitario y subtotal.

Todas las tablas incluyen datos iniciales de ejemplo. El historial de migraciones está en `Migrations/`.

### MySQL remoto (ADO.NET)

- `TBLCLIENTES` (base `dbfacturas`): `IdCliente` (PK), `StrNombre`, `NumDocumento`, `StrDireccion`, `StrTelefono`, `StrEmail`, `DtmFechaModifica`, `StrUsuarioModifica`.
- No usa migraciones de EF Core. La tabla debe existir en el servidor.

## Alcance y limitaciones

- No hay inicio de sesión ni permisos por rol. La auditoría usa un usuario fijo.
- Roles y Seguridad aparecen en el menú como "en construcción".
- El Dashboard no se desarrolla.
- El informe Financiero no tiene restricción de rol.

## Historial de cambios

- Proyecto base creado en net10.0 con EF Core 10.0.15.
- Migración del módulo de facturación: empleados, roles y páginas informativas.
- Módulo de Clientes con ADO.NET y MySQL.
- Fase 1: categorías y productos.
- Fase 3: facturas con cálculo de totales, control de stock y estados.
- Fase 5: informes en vivo con exportación CSV.
- Fase 2: retiro y reactivación de empleados, documento único de clientes y modal de confirmación.
- Eliminación del módulo de Personas.