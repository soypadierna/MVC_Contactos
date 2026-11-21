using Microsoft.EntityFrameworkCore;
using MVC_Contactos.Models;

namespace MVC_Contactos.Data
{
    public class ContactosDbContext : DbContext
    {
        public ContactosDbContext(DbContextOptions<ContactosDbContext> options)
            : base(options)
        {
        }

        public DbSet<ModeloPersona> Personas => Set<ModeloPersona>();
        public DbSet<TBLEMPLEADO> TBLEMPLEADO => Set<TBLEMPLEADO>();
        public DbSet<TBLROLES> TBLROLES => Set<TBLROLES>();
        public DbSet<TBLCATEGORIAS> TBLCATEGORIAS => Set<TBLCATEGORIAS>();
        public DbSet<TBLPRODUCTOS> TBLPRODUCTOS => Set<TBLPRODUCTOS>();
        public DbSet<TBLFACTURAS> TBLFACTURAS => Set<TBLFACTURAS>();
        public DbSet<TBLFACTURADETALLE> TBLFACTURADETALLE => Set<TBLFACTURADETALLE>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TBLCATEGORIAS>()
                .HasIndex(c => c.StrDescripcion)
                .IsUnique();

            modelBuilder.Entity<TBLPRODUCTOS>(e =>
            {
                e.HasIndex(p => p.StrCodigo).IsUnique();
                e.Property(p => p.DecPrecioCompra).HasPrecision(18, 2);
                e.Property(p => p.DecPrecioVenta).HasPrecision(18, 2);
                e.HasOne(p => p.Categoria)
                    .WithMany()
                    .HasForeignKey(p => p.IdCategoria)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TBLFACTURAS>(e =>
            {
                e.Property(f => f.DecSubtotal).HasPrecision(18, 2);
                e.Property(f => f.DecDescuento).HasPrecision(18, 2);
                e.Property(f => f.DecIva).HasPrecision(18, 2);
                e.Property(f => f.DecTotal).HasPrecision(18, 2);
                e.HasIndex(f => f.IdCliente);
                e.HasOne(f => f.Empleado)
                    .WithMany()
                    .HasForeignKey(f => f.IdEmpleado)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<TBLFACTURADETALLE>(e =>
            {
                e.Property(d => d.DecPrecioUnitario).HasPrecision(18, 2);
                e.Property(d => d.DecSubtotal).HasPrecision(18, 2);
                e.HasOne(d => d.Factura)
                    .WithMany(f => f.Detalles)
                    .HasForeignKey(d => d.IdFactura)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(d => d.Producto)
                    .WithMany()
                    .HasForeignKey(d => d.IdProducto)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ModeloPersona>().HasData(
                new ModeloPersona { PersonaId = 1, Nombre = "Juan", Telefono = "3333333", Email = "javiy2k@gmail.com" },
                new ModeloPersona { PersonaId = 2, Nombre = "pepe", Telefono = "2345678", Email = "pepe@gmail.com" },
                new ModeloPersona { PersonaId = 3, Nombre = "luis", Telefono = "444444", Email = "Luis@gmail.com" },
                new ModeloPersona { PersonaId = 4, Nombre = "Carmen", Telefono = "678868", Email = "Carmen@gmail.com" }
            );

            modelBuilder.Entity<TBLROLES>().HasData(
                new TBLROLES { IdRolEmpleado = 1, StrDescripcion = "Administrador" },
                new TBLROLES { IdRolEmpleado = 2, StrDescripcion = "Vendedor" },
                new TBLROLES { IdRolEmpleado = 3, StrDescripcion = "Cajero" }
            );

            var fecha = new DateTime(2026, 11, 21);

            modelBuilder.Entity<TBLEMPLEADO>().HasData(
                new TBLEMPLEADO { IdEmpleado = 1, strNombre = "Laura Gómez", NumDocumento = 1012345678, StrDireccion = "Cra 10 # 20-30", StrTelefono = "3001112233", StrEmail = "laura.gomez@empresa.com", IdRolEmpleado = 1, DtmIngreso = new DateTime(2023, 2, 1), strDatosAdicionales = "Administradora general", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLEMPLEADO { IdEmpleado = 2, strNombre = "Carlos Ruiz", NumDocumento = 1023456789, StrDireccion = "Calle 45 # 12-08", StrTelefono = "3012223344", StrEmail = "carlos.ruiz@empresa.com", IdRolEmpleado = 2, DtmIngreso = new DateTime(2023, 6, 15), strDatosAdicionales = "Ventas zona norte", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLEMPLEADO { IdEmpleado = 3, strNombre = "Marta Pérez", NumDocumento = 1034567890, StrDireccion = "Av. 68 # 5-50", StrTelefono = "3023334455", StrEmail = "marta.perez@empresa.com", IdRolEmpleado = 3, DtmIngreso = new DateTime(2024, 1, 10), strDatosAdicionales = "Caja principal", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLEMPLEADO { IdEmpleado = 4, strNombre = "Andrés Torres", NumDocumento = 1045678901, StrDireccion = "Cra 7 # 72-14", StrTelefono = "3034445566", StrEmail = "andres.torres@empresa.com", IdRolEmpleado = 2, DtmIngreso = new DateTime(2024, 8, 5), strDatosAdicionales = "Ventas zona sur", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLEMPLEADO { IdEmpleado = 5, strNombre = "Sofía Herrera", NumDocumento = 1056789012, StrDireccion = "Calle 100 # 15-20", StrTelefono = "3045556677", StrEmail = "sofia.herrera@empresa.com", IdRolEmpleado = 3, DtmIngreso = new DateTime(2022, 3, 20), DtmRetiro = new DateTime(2025, 12, 31), strDatosAdicionales = "Retirada por fin de contrato", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" }
            );

            modelBuilder.Entity<TBLCATEGORIAS>().HasData(
                new TBLCATEGORIAS { IdCategoria = 1, StrDescripcion = "Bebidas", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLCATEGORIAS { IdCategoria = 2, StrDescripcion = "Snacks", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLCATEGORIAS { IdCategoria = 3, StrDescripcion = "Aseo", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLCATEGORIAS { IdCategoria = 4, StrDescripcion = "Papelería", DtmFechaModifica = fecha, StrUsuarioModifico = "javier" }
            );

            modelBuilder.Entity<TBLPRODUCTOS>().HasData(
                new TBLPRODUCTOS { IdProducto = 1, StrNombre = "Agua mineral 600 ml", StrCodigo = "AGU-001", DecPrecioCompra = 900m, DecPrecioVenta = 1500m, IdCategoria = 1, StrDetalle = "Botella PET", IntStock = 120, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 2, StrNombre = "Gaseosa cola 1.5 L", StrCodigo = "GAS-001", DecPrecioCompra = 3200m, DecPrecioVenta = 4800m, IdCategoria = 1, StrDetalle = "Botella familiar", IntStock = 60, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 3, StrNombre = "Jugo de naranja 1 L", StrCodigo = "JUG-001", DecPrecioCompra = 2800m, DecPrecioVenta = 4200m, IdCategoria = 1, StrDetalle = "Caja tetrapak", IntStock = 45, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 4, StrNombre = "Papas fritas 150 g", StrCodigo = "SNK-001", DecPrecioCompra = 2100m, DecPrecioVenta = 3500m, IdCategoria = 2, StrDetalle = "Sabor natural", IntStock = 80, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 5, StrNombre = "Galletas de chocolate", StrCodigo = "SNK-002", DecPrecioCompra = 1200m, DecPrecioVenta = 2000m, IdCategoria = 2, StrDetalle = "Paquete x6", IntStock = 100, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 6, StrNombre = "Detergente en polvo 1 kg", StrCodigo = "ASE-001", DecPrecioCompra = 6500m, DecPrecioVenta = 9800m, IdCategoria = 3, StrDetalle = "Ropa blanca y de color", IntStock = 30, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 7, StrNombre = "Jabón de manos 500 ml", StrCodigo = "ASE-002", DecPrecioCompra = 3000m, DecPrecioVenta = 5200m, IdCategoria = 3, StrDetalle = "Antibacterial", IntStock = 40, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 8, StrNombre = "Cuaderno rayado 100 hojas", StrCodigo = "PAP-001", DecPrecioCompra = 2500m, DecPrecioVenta = 4500m, IdCategoria = 4, StrDetalle = "Pasta blanda", IntStock = 3, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLPRODUCTOS { IdProducto = 9, StrNombre = "Lapicero azul", StrCodigo = "PAP-002", DecPrecioCompra = 600m, DecPrecioVenta = 1200m, IdCategoria = 4, StrDetalle = "Punta media", IntStock = 200, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" }
            );

            modelBuilder.Entity<TBLFACTURAS>().HasData(
                new TBLFACTURAS { IdFactura = 1, IdCliente = 1, IdEmpleado = 2, DtmFecha = new DateTime(2026, 11, 18, 15, 0, 0), IdEstado = 2, DecSubtotal = 32500m, DecDescuento = 0m, DecIva = 6175m, DecTotal = 38675m, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLFACTURAS { IdFactura = 2, IdCliente = 2, IdEmpleado = 4, DtmFecha = new DateTime(2026, 11, 19, 16, 30, 0), IdEstado = 1, DecSubtotal = 35200m, DecDescuento = 3520m, DecIva = 6019.20m, DecTotal = 37699.20m, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLFACTURAS { IdFactura = 3, IdCliente = 1, IdEmpleado = 2, DtmFecha = new DateTime(2026, 11, 20, 14, 15, 0), IdEstado = 3, DecSubtotal = 19200m, DecDescuento = 0m, DecIva = 3648m, DecTotal = 22848m, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" },
                new TBLFACTURAS { IdFactura = 4, IdCliente = 3, IdEmpleado = 3, DtmFecha = new DateTime(2026, 10, 25, 17, 0, 0), IdEstado = 4, DecSubtotal = 16500m, DecDescuento = 825m, DecIva = 2978.25m, DecTotal = 18653.25m, DtmFechaModifica = fecha, StrUsuarioModifico = "javier" }
            );

            modelBuilder.Entity<TBLFACTURADETALLE>().HasData(
                new TBLFACTURADETALLE { IdDetalle = 1, IdFactura = 1, IdProducto = 1, IntCantidad = 10, DecPrecioUnitario = 1500m, DecSubtotal = 15000m },
                new TBLFACTURADETALLE { IdDetalle = 2, IdFactura = 1, IdProducto = 4, IntCantidad = 5, DecPrecioUnitario = 3500m, DecSubtotal = 17500m },
                new TBLFACTURADETALLE { IdDetalle = 3, IdFactura = 2, IdProducto = 6, IntCantidad = 2, DecPrecioUnitario = 9800m, DecSubtotal = 19600m },
                new TBLFACTURADETALLE { IdDetalle = 4, IdFactura = 2, IdProducto = 7, IntCantidad = 3, DecPrecioUnitario = 5200m, DecSubtotal = 15600m },
                new TBLFACTURADETALLE { IdDetalle = 5, IdFactura = 3, IdProducto = 2, IntCantidad = 4, DecPrecioUnitario = 4800m, DecSubtotal = 19200m },
                new TBLFACTURADETALLE { IdDetalle = 6, IdFactura = 4, IdProducto = 8, IntCantidad = 1, DecPrecioUnitario = 4500m, DecSubtotal = 4500m },
                new TBLFACTURADETALLE { IdDetalle = 7, IdFactura = 4, IdProducto = 9, IntCantidad = 10, DecPrecioUnitario = 1200m, DecSubtotal = 12000m }
            );
        }
    }
}