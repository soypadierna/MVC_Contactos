using Microsoft.AspNetCore.Mvc;
using MVC_Contactos.Data;
using MVC_Contactos.Models;

namespace MVC_Contactos.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteDatos _datos;
        private readonly ContactosDbContext _context;

        public ClientesController(IConfiguration configuration, ContactosDbContext context)
        {
            _datos = new ClienteDatos(configuration);
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_datos.ListarClientes());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Cliente());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Cliente cliente)
        {
            if (cliente.NumDocumento.HasValue && _datos.ExisteDocumento(cliente.NumDocumento.Value))
                ModelState.AddModelError("NumDocumento", "Ya existe un cliente con ese documento.");

            if (!ModelState.IsValid)
            {
                return View(cliente);
            }
            cliente.DtmFechaModifica = DateTime.Now;
            cliente.StrUsuarioModifica = "Sistema";
            _datos.InsertarCliente(cliente);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var cliente = _datos.ObtenerCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Cliente cliente)
        {
            if (cliente.NumDocumento.HasValue && _datos.ExisteDocumento(cliente.NumDocumento.Value, cliente.IdCliente))
                ModelState.AddModelError("NumDocumento", "Ya existe un cliente con ese documento.");

            if (!ModelState.IsValid)
            {
                return View(cliente);
            }
            cliente.DtmFechaModifica = DateTime.Now;
            cliente.StrUsuarioModifica = "Sistema";
            _datos.ActualizarCliente(cliente);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (_datos.ObtenerCliente(id) == null)
            {
                return NotFound();
            }
            if (TieneFacturas(id))
            {
                TempData["Error"] = "No se puede eliminar: el cliente tiene facturas.";
                return RedirectToAction("Index");
            }
            _datos.EliminarCliente(id);
            TempData["Ok"] = "Cliente eliminado.";
            return RedirectToAction("Index");
        }

        private bool TieneFacturas(int idCliente)
        {
            return _context.TBLFACTURAS.Any(f => f.IdCliente == idCliente);
        }
    }
}