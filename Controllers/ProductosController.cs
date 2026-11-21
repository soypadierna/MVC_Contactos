using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Contactos.Data;
using MVC_Contactos.Models;

namespace MVC_Contactos.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ContactosDbContext _context;
        private readonly IWebHostEnvironment _env;
        private static readonly string[] Extensiones = { ".jpg", ".jpeg", ".png", ".webp" };
        private const long MaxFoto = 3 * 1024 * 1024;

        public ProductosController(ContactosDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index(string? q)
        {
            var lista = _context.TBLPRODUCTOS.Include(p => p.Categoria).AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                var t = q.Trim();
                lista = lista.Where(p => p.StrNombre.Contains(t) || p.StrCodigo.Contains(t));
            }
            ViewBag.Q = q;
            return View(lista.OrderBy(p => p.StrNombre).ToList());
        }

        public IActionResult Nuevo()
        {
            CargarCategorias(null);
            return View(new TBLPRODUCTOS());
        }

        [HttpPost]
        public IActionResult Nuevo(TBLPRODUCTOS model, IFormFile? foto)
        {
            Normalizar(model);
            Validar(model, foto);

            if (!ModelState.IsValid)
            {
                CargarCategorias(model.IdCategoria);
                return View(model);
            }

            string? nombreFoto = null;
            try
            {
                if (foto != null && foto.Length > 0) nombreFoto = GuardarFoto(foto);
                model.StrFoto = nombreFoto;
                model.DtmFechaModifica = DateTime.UtcNow;
                model.StrUsuarioModifico = "javier";
                _context.TBLPRODUCTOS.Add(model);
                _context.SaveChanges();
            }
            catch
            {
                BorrarFoto(nombreFoto);
                throw;
            }
            TempData["Ok"] = "Producto creado.";
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            var prod = _context.TBLPRODUCTOS.Find(id);
            if (prod == null) return NotFound();
            CargarCategorias(prod.IdCategoria);
            return View(prod);
        }

        [HttpPost]
        public IActionResult Editar(TBLPRODUCTOS model, IFormFile? foto, bool quitarFoto)
        {
            Normalizar(model);
            Validar(model, foto);

            var prod = _context.TBLPRODUCTOS.Find(model.IdProducto);
            if (prod == null) return NotFound();

            if (!ModelState.IsValid)
            {
                model.StrFoto = prod.StrFoto;
                CargarCategorias(model.IdCategoria);
                return View(model);
            }

            var fotoAnterior = prod.StrFoto;
            string? fotoNueva = null;
            try
            {
                if (foto != null && foto.Length > 0)
                {
                    fotoNueva = GuardarFoto(foto);
                    prod.StrFoto = fotoNueva;
                }
                else if (quitarFoto)
                {
                    prod.StrFoto = null;
                }

                prod.StrNombre = model.StrNombre;
                prod.StrCodigo = model.StrCodigo;
                prod.DecPrecioCompra = model.DecPrecioCompra;
                prod.DecPrecioVenta = model.DecPrecioVenta;
                prod.IdCategoria = model.IdCategoria;
                prod.StrDetalle = model.StrDetalle;
                prod.IntStock = model.IntStock;
                prod.DtmFechaModifica = DateTime.UtcNow;
                prod.StrUsuarioModifico = "javier";
                _context.SaveChanges();
            }
            catch
            {
                BorrarFoto(fotoNueva);
                throw;
            }

            if (prod.StrFoto != fotoAnterior) BorrarFoto(fotoAnterior);
            TempData["Ok"] = "Producto actualizado.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Borrar(int id)
        {
            var prod = _context.TBLPRODUCTOS.Find(id);
            if (prod == null) return NotFound();
            var foto = prod.StrFoto;
            try
            {
                _context.TBLPRODUCTOS.Remove(prod);
                _context.SaveChanges();
                BorrarFoto(foto);
                TempData["Ok"] = "Producto eliminado.";
            }
            catch (DbUpdateException)
            {
                TempData["Error"] = "No se puede eliminar: el producto está siendo usado.";
            }
            return RedirectToAction("Index");
        }

        private void Normalizar(TBLPRODUCTOS m)
        {
            m.StrNombre = (m.StrNombre ?? "").Trim();
            m.StrCodigo = (m.StrCodigo ?? "").Trim();
            m.StrDetalle = string.IsNullOrWhiteSpace(m.StrDetalle) ? null : m.StrDetalle.Trim();
        }

        private void Validar(TBLPRODUCTOS m, IFormFile? foto)
        {
            var codigo = m.StrCodigo.ToLower();
            if (codigo.Length > 0 &&
                _context.TBLPRODUCTOS.Any(p => p.IdProducto != m.IdProducto && p.StrCodigo.ToLower() == codigo))
                ModelState.AddModelError("StrCodigo", "Ya existe un producto con ese código.");

            if (m.IdCategoria != null && !_context.TBLCATEGORIAS.Any(c => c.IdCategoria == m.IdCategoria))
                ModelState.AddModelError("IdCategoria", "La categoría seleccionada no existe.");

            if (decimal.Round(m.DecPrecioCompra, 2) != m.DecPrecioCompra)
                ModelState.AddModelError("DecPrecioCompra", "Máximo 2 decimales.");
            if (decimal.Round(m.DecPrecioVenta, 2) != m.DecPrecioVenta)
                ModelState.AddModelError("DecPrecioVenta", "Máximo 2 decimales.");

            var errorFoto = ValidarFoto(foto);
            if (errorFoto != null) ModelState.AddModelError("foto", errorFoto);
        }

        private void CargarCategorias(int? seleccionada)
        {
            ViewBag.IdCategoria = new SelectList(
                _context.TBLCATEGORIAS.OrderBy(c => c.StrDescripcion), "IdCategoria", "StrDescripcion", seleccionada);
        }

        private static string? ValidarFoto(IFormFile? foto)
        {
            if (foto == null || foto.Length == 0) return null;
            if (foto.Length > MaxFoto) return "La foto no puede superar 3 MB.";
            var ext = Path.GetExtension(foto.FileName).ToLowerInvariant();
            if (!Extensiones.Contains(ext)) return "Solo se permiten fotos JPG, PNG o WebP.";

            var b = new byte[12];
            using (var s = foto.OpenReadStream())
            {
                if (s.Read(b, 0, 12) < 12) return "El archivo no es una imagen válida.";
            }
            bool jpg = b[0] == 0xFF && b[1] == 0xD8 && b[2] == 0xFF;
            bool png = b[0] == 0x89 && b[1] == 0x50 && b[2] == 0x4E && b[3] == 0x47;
            bool webp = b[0] == 0x52 && b[1] == 0x49 && b[2] == 0x46 && b[3] == 0x46
                     && b[8] == 0x57 && b[9] == 0x45 && b[10] == 0x42 && b[11] == 0x50;
            return (jpg || png || webp) ? null : "El archivo no es una imagen válida.";
        }

        private string GuardarFoto(IFormFile foto)
        {
            var carpeta = Path.Combine(_env.WebRootPath, "uploads");
            Directory.CreateDirectory(carpeta);
            var nombre = Guid.NewGuid().ToString("N") + Path.GetExtension(foto.FileName).ToLowerInvariant();
            using var fs = new FileStream(Path.Combine(carpeta, nombre), FileMode.Create);
            foto.CopyTo(fs);
            return nombre;
        }

        private void BorrarFoto(string? nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return;
            try
            {
                var ruta = Path.Combine(_env.WebRootPath, "uploads", Path.GetFileName(nombre));
                if (System.IO.File.Exists(ruta)) System.IO.File.Delete(ruta);
            }
            catch { }
        }
    }
}