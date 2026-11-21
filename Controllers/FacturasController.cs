using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Contactos.Data;
using MVC_Contactos.Models;

namespace MVC_Contactos.Controllers
{
    public class FacturasController : Controller
    {
        private readonly ContactosDbContext _context;
        private readonly ClienteDatos _clientes;
        private readonly decimal _iva;
        private const int MaxLineas = 100;

        private static readonly Dictionary<int, int[]> Transiciones = new()
        {
            { 1, new[] { 2, 3, 4 } },
            { 2, new[] { 3 } },
            { 3, Array.Empty<int>() },
            { 4, new[] { 2, 3 } }
        };

        public FacturasController(ContactosDbContext context, IConfiguration configuration)
        {
            _context = context;
            _clientes = new ClienteDatos(configuration);
            _iva = configuration.GetValue<decimal>("Facturacion:IvaPorcentaje", 19m);
        }

        public IActionResult Index(string? q)
        {
            var nombres = _clientes.ListarClientes().ToDictionary(c => c.IdCliente, c => c.StrNombre);
            var query = _context.TBLFACTURAS.Include(f => f.Empleado).AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim();
                var ids = nombres.Where(n => n.Value.Contains(t, StringComparison.OrdinalIgnoreCase))
                                 .Select(n => n.Key).ToList();
                int.TryParse(t, out var num);
                query = query.Where(f => f.IdFactura == num || ids.Contains(f.IdCliente));
            }
            ViewBag.Q = q;
            ViewBag.Nombres = nombres;
            return View(query.OrderByDescending(f => f.IdFactura).ToList());
        }

        public IActionResult Nuevo()
        {
            var vm = new FacturaNuevaVM();
            Cargar(vm);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Nuevo(FacturaNuevaVM vm)
        {
            vm.Lineas ??= new List<LineaFacturaVM>();
            Validar(vm);
            if (!ModelState.IsValid)
            {
                Cargar(vm);
                return View(vm);
            }

            using var tx = _context.Database.BeginTransaction();

            var grupos = vm.Lineas
                .GroupBy(l => l.IdProducto)
                .Select(g => new { IdProducto = g.Key, Cantidad = g.Sum(x => x.Cantidad) })
                .OrderBy(g => g.IdProducto)
                .ToList();

            var productos = BloquearProductos(grupos.Select(g => g.IdProducto)).ToDictionary(p => p.IdProducto);

            var errores = new List<string>();
            foreach (var g in grupos)
            {
                if (!productos.TryGetValue(g.IdProducto, out var p))
                    errores.Add("Un producto seleccionado ya no existe.");
                else if (p.IntStock < g.Cantidad)
                    errores.Add($"Stock insuficiente para {p.StrNombre} (disponible: {p.IntStock}).");
            }
            if (errores.Count > 0)
            {
                tx.Rollback();
                foreach (var e in errores) ModelState.AddModelError("", e);
                Cargar(vm);
                return View(vm);
            }

            var detalles = new List<TBLFACTURADETALLE>();
            foreach (var g in grupos)
            {
                var p = productos[g.IdProducto];
                p.IntStock -= g.Cantidad;
                detalles.Add(new TBLFACTURADETALLE
                {
                    IdProducto = p.IdProducto,
                    IntCantidad = g.Cantidad,
                    DecPrecioUnitario = p.DecPrecioVenta,
                    DecSubtotal = Redondear(p.DecPrecioVenta * g.Cantidad)
                });
            }

            var subtotal = detalles.Sum(d => d.DecSubtotal);
            var descuento = Redondear(subtotal * vm.DescuentoPorcentaje / 100m);
            var baseIva = subtotal - descuento;
            var iva = Redondear(baseIva * _iva / 100m);

            var factura = new TBLFACTURAS
            {
                IdCliente = vm.IdCliente!.Value,
                IdEmpleado = vm.IdEmpleado!.Value,
                DtmFecha = DateTime.UtcNow,
                IdEstado = vm.IdEstado,
                DecSubtotal = subtotal,
                DecDescuento = descuento,
                DecIva = iva,
                DecTotal = baseIva + iva,
                DtmFechaModifica = DateTime.UtcNow,
                StrUsuarioModifico = "javier",
                Detalles = detalles
            };
            _context.TBLFACTURAS.Add(factura);
            _context.SaveChanges();
            tx.Commit();

            TempData["Ok"] = $"Factura {factura.IdFactura} creada.";
            return RedirectToAction("Detalle", new { id = factura.IdFactura });
        }

        public IActionResult Detalle(int id)
        {
            var f = _context.TBLFACTURAS
                .Include(x => x.Empleado)
                .Include(x => x.Detalles).ThenInclude(d => d.Producto)
                .FirstOrDefault(x => x.IdFactura == id);
            if (f == null) return NotFound();

            ViewBag.Cliente = _clientes.ObtenerCliente(f.IdCliente);
            ViewBag.Acciones = Transiciones.TryGetValue(f.IdEstado, out var a) ? a : Array.Empty<int>();
            return View(f);
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, int nuevoEstado)
        {
            using var tx = _context.Database.BeginTransaction();

            var f = _context.TBLFACTURAS
                .FromSqlInterpolated($"SELECT * FROM TBLFACTURAS WITH (UPDLOCK, ROWLOCK) WHERE IdFactura = {id}")
                .Include(x => x.Detalles)
                .FirstOrDefault();
            if (f == null) return NotFound();

            if (!Transiciones.TryGetValue(f.IdEstado, out var permitidos) || !permitidos.Contains(nuevoEstado))
            {
                TempData["Error"] = "Ese cambio de estado no está permitido.";
                return RedirectToAction("Detalle", new { id });
            }

            if (nuevoEstado == 3)
            {
                var productos = BloquearProductos(f.Detalles.Select(d => d.IdProducto)).ToDictionary(p => p.IdProducto);
                foreach (var d in f.Detalles)
                {
                    if (productos.TryGetValue(d.IdProducto, out var p)) p.IntStock += d.IntCantidad;
                }
            }

            f.IdEstado = nuevoEstado;
            f.DtmFechaModifica = DateTime.UtcNow;
            f.StrUsuarioModifico = "javier";
            _context.SaveChanges();
            tx.Commit();

            TempData["Ok"] = "Estado actualizado.";
            return RedirectToAction("Detalle", new { id });
        }

        private void Validar(FacturaNuevaVM vm)
        {
            if (vm.IdCliente != null && _clientes.ObtenerCliente(vm.IdCliente.Value) == null)
                ModelState.AddModelError("IdCliente", "El cliente no existe.");

            if (vm.IdEmpleado != null)
            {
                var hoy = DateTime.Today;
                if (!_context.TBLEMPLEADO.Any(e => e.IdEmpleado == vm.IdEmpleado && (e.DtmRetiro == null || e.DtmRetiro > hoy)))
                    ModelState.AddModelError("IdEmpleado", "El empleado no existe o está retirado.");
            }

            if (vm.IdEstado != 1 && vm.IdEstado != 2)
                ModelState.AddModelError("IdEstado", "Estado inicial no válido.");

            if (vm.Lineas.Count == 0)
                ModelState.AddModelError("", "Agregue al menos un producto.");
            else if (vm.Lineas.Count > MaxLineas)
                ModelState.AddModelError("", $"Máximo {MaxLineas} líneas por factura.");
            else if (vm.Lineas.Any(l => l.IdProducto <= 0 || l.Cantidad < 1 || l.Cantidad > 1000000))
                ModelState.AddModelError("", "Cada línea necesita un producto y una cantidad entera entre 1 y 1.000.000.");
        }

        private void Cargar(FacturaNuevaVM vm)
        {
            ViewBag.IdCliente = new SelectList(
                _clientes.ListarClientes().OrderBy(c => c.StrNombre), "IdCliente", "StrNombre", vm.IdCliente);

            var hoy = DateTime.Today;
            ViewBag.IdEmpleado = new SelectList(
                _context.TBLEMPLEADO.Where(e => e.DtmRetiro == null || e.DtmRetiro > hoy).OrderBy(e => e.strNombre),
                "IdEmpleado", "strNombre", vm.IdEmpleado);

            ViewBag.Catalogo = _context.TBLPRODUCTOS
                .Where(p => p.IntStock > 0)
                .OrderBy(p => p.StrNombre)
                .Select(p => new { id = p.IdProducto, nombre = p.StrNombre, codigo = p.StrCodigo, precio = p.DecPrecioVenta, stock = p.IntStock })
                .ToList();

            ViewBag.Iva = _iva;
        }

#pragma warning disable EF1002
        private List<TBLPRODUCTOS> BloquearProductos(IEnumerable<int> ids)
        {
            var lista = ids.Distinct().OrderBy(i => i).ToList();
            if (lista.Count == 0) return new List<TBLPRODUCTOS>();
            var inList = string.Join(",", lista);
            return _context.TBLPRODUCTOS
                .FromSqlRaw($"SELECT * FROM TBLPRODUCTOS WITH (UPDLOCK, ROWLOCK) WHERE IdProducto IN ({inList}) ORDER BY IdProducto")
                .ToList();
        }
#pragma warning restore EF1002

        private static decimal Redondear(decimal v) => Math.Round(v, 2, MidpointRounding.AwayFromZero);
    }
}