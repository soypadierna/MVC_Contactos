using Microsoft.AspNetCore.Mvc;
using MVC_Contactos.Data;
using MVC_Contactos.Models;

namespace MVC_Contactos.Controllers
{
    public class CRUD_PersonaController : Controller
    {
        private readonly ContactosDbContext _context;

        public CRUD_PersonaController(ContactosDbContext context)
        {
            _context = context;
        }

        // LISTADO DE LAS PERSONAS-PAGINA PRINCIPAL
        public IActionResult Index()
        {
            ViewBag.Titulo = "ADMIN PERSONAS";
            return View(_context.Personas.ToList());
        }

        // GET: NUEVO REGISTRO- formulario en blanco
        public IActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Nuevo(IFormCollection collection)
        {
            var Persona = new ModeloPersona
            {
                Nombre = collection["Nombre"].ToString(),
                Telefono = collection["Telefono"].ToString(),
                Email = collection["Email"].ToString()
            };
            _context.Personas.Add(Persona);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: CRUD_Persona editar
        public IActionResult Editar(int id = 0)
        {
            var Persona = _context.Personas.Find(id);
            if (Persona == null) return NotFound();
            return View(Persona);
        }

        [HttpPost]
        public IActionResult Editar(IFormCollection collection)
        {
            var Persona = _context.Personas.Find(int.Parse(collection["PersonaId"].ToString()));
            if (Persona == null) return NotFound();
            Persona.Nombre = collection["Nombre"].ToString();
            Persona.Telefono = collection["Telefono"].ToString();
            Persona.Email = collection["Email"].ToString();
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Borrado de registro
        public IActionResult Borrar(int id = 0)
        {
            var Persona = _context.Personas.Find(id);
            if (Persona == null) return NotFound();
            return View(Persona);
        }

        [HttpPost]
        public IActionResult BorrarConfirmado(IFormCollection collection)
        {
            var Persona = _context.Personas.Find(int.Parse(collection["PersonaId"].ToString()));
            if (Persona == null) return NotFound();
            _context.Personas.Remove(Persona);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}