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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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
        }
    }
}