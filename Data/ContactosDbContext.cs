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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ModeloPersona>().HasData(
                new ModeloPersona { PersonaId = 1, Nombre = "Juan", Telefono = "3333333", Email = "javiy2k@gmail.com" },
                new ModeloPersona { PersonaId = 2, Nombre = "pepe", Telefono = "2345678", Email = "pepe@gmail.com" },
                new ModeloPersona { PersonaId = 3, Nombre = "luis", Telefono = "444444", Email = "Luis@gmail.com" },
                new ModeloPersona { PersonaId = 4, Nombre = "Carmen", Telefono = "678868", Email = "Carmen@gmail.com" }
            );
        }
    }
}