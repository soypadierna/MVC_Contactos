using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_Contactos.Data;
using MVC_Contactos.Models;

namespace MVC_Contactos.Controllers
{
    public class TBLempleadoController : Controller
    {
        private readonly ContactosDbContext _context;

        public TBLempleadoController(ContactosDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string? q, bool verRetirados = false)
        {
            var hoy = DateTime.Today;
            var query = _context.TBLEMPLEADO.AsQueryable();

            if (!verRetirados)
                query = query.Where(e => e.DtmRetiro == null || e.DtmRetiro > hoy);

            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim();
                query = query.Where(e =>
                    (e.strNombre != null && e.strNombre.Contains(t)) ||
                    e.NumDocumento.ToString().Contains(t) ||
                    (e.StrEmail != null && e.StrEmail.Contains(t)));
            }

            ViewBag.Q = q;
            ViewBag.VerRetirados = verRetirados;
            return View(query.OrderBy(e => e.strNombre).ToList());
        }

        public IActionResult Nuevo()
        {
            ViewBag.IdRolEmpleado = new SelectList(_context.TBLROLES, "IdRolEmpleado", "StrDescripcion");
            return View();
        }

        [HttpPost]
        public IActionResult Nuevo(TBLEMPLEADO model)
        {
            if (ModelState.IsValid)
            {
                model.DtmFechaModifica = DateTime.Now.Date;
                model.StrUsuarioModifico = "javier";
                _context.TBLEMPLEADO.Add(model);
                _context.SaveChanges();
                return Redirect("/TBLEmpleado/index");
            }
            ViewBag.IdRolEmpleado = new SelectList(_context.TBLROLES, "IdRolEmpleado", "StrDescripcion", model.IdRolEmpleado);
            return View(model);
        }

        public IActionResult Editar(int id)
        {
            var DatosEmple = _context.TBLEMPLEADO.Find(id);
            if (DatosEmple == null) return NotFound();
            ViewBag.IdRolEmpleado = new SelectList(_context.TBLROLES, "IdRolEmpleado", "StrDescripcion", DatosEmple.IdRolEmpleado);
            return View(DatosEmple);
        }

        [HttpPost]
        public IActionResult Editar(TBLEMPLEADO model)
        {
            if (ModelState.IsValid)
            {
                var DatosModifi = _context.TBLEMPLEADO.Find(model.IdEmpleado);
                if (DatosModifi == null) return NotFound();
                DatosModifi.strNombre = model.strNombre;
                DatosModifi.NumDocumento = model.NumDocumento;
                DatosModifi.StrDireccion = model.StrDireccion;
                DatosModifi.StrTelefono = model.StrTelefono;
                DatosModifi.StrEmail = model.StrEmail;
                DatosModifi.IdRolEmpleado = model.IdRolEmpleado;
                DatosModifi.DtmIngreso = model.DtmIngreso;
                DatosModifi.DtmRetiro = model.DtmRetiro;
                DatosModifi.strDatosAdicionales = model.strDatosAdicionales;
                DatosModifi.DtmFechaModifica = DateTime.Now.Date;
                DatosModifi.StrUsuarioModifico = "javier";
                _context.SaveChanges();
                return Redirect("/TBLEmpleado/index");
            }
            ViewBag.IdRolEmpleado = new SelectList(_context.TBLROLES, "IdRolEmpleado", "StrDescripcion", model.IdRolEmpleado);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Retirar(int id)
        {
            var e = _context.TBLEMPLEADO.Find(id);
            if (e == null) return NotFound();
            e.DtmRetiro = DateTime.Today;
            e.DtmFechaModifica = DateTime.Now.Date;
            e.StrUsuarioModifico = "javier";
            _context.SaveChanges();
            TempData["Ok"] = "Empleado retirado.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Reactivar(int id)
        {
            var e = _context.TBLEMPLEADO.Find(id);
            if (e == null) return NotFound();
            e.DtmRetiro = null;
            e.DtmFechaModifica = DateTime.Now.Date;
            e.StrUsuarioModifico = "javier";
            _context.SaveChanges();
            TempData["Ok"] = "Empleado reactivado.";
            return RedirectToAction("Index", new { verRetirados = true });
        }
    }
}