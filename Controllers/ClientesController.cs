using Microsoft.AspNetCore.Mvc;
using MVC_Contactos.Models;

namespace MVC_Contactos.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteDatos _datos;

        public ClientesController(IConfiguration configuration)
        {
            _datos = new ClienteDatos(configuration);
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
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }
            cliente.DtmFechaModifica = DateTime.Now;
            cliente.StrUsuarioModifica = "Sistema";
            _datos.ActualizarCliente(cliente);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
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
        public IActionResult DeleteConfirmed(int id)
        {
            _datos.EliminarCliente(id);
            return RedirectToAction("Index");
        }
    }
}