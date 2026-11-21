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

        public IActionResult Index()
        {
            ViewBag.Titulo = "Lista de Personas";
            return View(_context.Personas.ToList());
        }

        public IActionResult Nuevo()
        {
            return View(new ModeloPersona());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Nuevo(ModeloPersona model)
        {
            if (!ModelState.IsValid) return View(model);
            model.PersonaId = 0;
            _context.Personas.Add(model);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id = 0)
        {
            var Persona = _context.Personas.Find(id);
            if (Persona == null) return NotFound();
            return View(Persona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(ModeloPersona model)
        {
            if (!ModelState.IsValid) return View(model);
            var Persona = _context.Personas.Find(model.PersonaId);
            if (Persona == null) return NotFound();
            Persona.Nombre = model.Nombre;
            Persona.Telefono = model.Telefono;
            Persona.Email = model.Email;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Borrar(int id = 0)
        {
            var Persona = _context.Personas.Find(id);
            if (Persona == null) return NotFound();
            return View(Persona);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BorrarConfirmado(int PersonaId)
        {
            var Persona = _context.Personas.Find(PersonaId);
            if (Persona == null) return NotFound();
            _context.Personas.Remove(Persona);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}