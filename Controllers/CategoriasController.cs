using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVC_Contactos.Data;
using MVC_Contactos.Models;

namespace MVC_Contactos.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ContactosDbContext _context;

        public CategoriasController(ContactosDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? q)
        {
            var lista = _context.TBLCATEGORIAS.AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim();
                lista = lista.Where(c => c.StrDescripcion.Contains(t));
            }
            ViewBag.Q = q;
            return View(lista.OrderBy(c => c.StrDescripcion).ToList());
        }

        public IActionResult Nuevo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Nuevo(TBLCATEGORIAS model)
        {
            model.StrDescripcion = (model.StrDescripcion ?? "").Trim();
            if (Existe(model.StrDescripcion, 0))
                ModelState.AddModelError("StrDescripcion", "Ya existe una categoría con esa descripción.");

            if (!ModelState.IsValid) return View(model);

            model.DtmFechaModifica = DateTime.UtcNow;
            model.StrUsuarioModifico = "javier";
            _context.TBLCATEGORIAS.Add(model);
            _context.SaveChanges();
            TempData["Ok"] = "Categoría creada.";
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            var cat = _context.TBLCATEGORIAS.Find(id);
            if (cat == null) return NotFound();
            return View(cat);
        }

        [HttpPost]
        public IActionResult Editar(TBLCATEGORIAS model)
        {
            model.StrDescripcion = (model.StrDescripcion ?? "").Trim();
            if (Existe(model.StrDescripcion, model.IdCategoria))
                ModelState.AddModelError("StrDescripcion", "Ya existe una categoría con esa descripción.");

            if (!ModelState.IsValid) return View(model);

            var cat = _context.TBLCATEGORIAS.Find(model.IdCategoria);
            if (cat == null) return NotFound();
            cat.StrDescripcion = model.StrDescripcion;
            cat.DtmFechaModifica = DateTime.UtcNow;
            cat.StrUsuarioModifico = "javier";
            _context.SaveChanges();
            TempData["Ok"] = "Categoría actualizada.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Borrar(int id)
        {
            var cat = _context.TBLCATEGORIAS.Find(id);
            if (cat == null) return NotFound();
            try
            {
                _context.TBLCATEGORIAS.Remove(cat);
                _context.SaveChanges();
                TempData["Ok"] = "Categoría eliminada.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "No se puede eliminar: la categoría tiene productos asociados.";
            }
            return RedirectToAction("Index");
        }

        private bool Existe(string descripcion, int idActual)
        {
            var d = descripcion.ToLower();
            return _context.TBLCATEGORIAS.Any(c => c.IdCategoria != idActual && c.StrDescripcion.ToLower() == d);
        }
    }
}