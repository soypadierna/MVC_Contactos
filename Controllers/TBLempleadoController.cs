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

        public IActionResult Index()
        {
            return View(_context.TBLEMPLEADO.ToList());
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

        [HttpGet]
        public IActionResult Borrar(int id)
        {
            var otabla = _context.TBLEMPLEADO.Find(id);
            if (otabla == null) return NotFound();
            if (_context.TBLFACTURAS.Any(f => f.IdEmpleado == id))
            {
                TempData["Error"] = "No se puede eliminar: el empleado tiene facturas.";
                return Redirect("/TBLEmpleado/index");
            }
            _context.TBLEMPLEADO.Remove(otabla);
            _context.SaveChanges();
            return Redirect("/TBLEmpleado/index");
        }
    }
}