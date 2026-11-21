using Microsoft.AspNetCore.Mvc;
using MVC_Contactos.Models;
using MVC_Contactos.Services;

namespace MVC_Contactos.Controllers
{
    // TODO Fase 4: [Permiso("informes")] + permiso del tema; Financiero solo rol 1
    public class InformesController : Controller
    {
        private readonly InformesService _svc;

        public InformesController(InformesService svc)
        {
            _svc = svc;
        }

        public IActionResult Index(InformeVM vm, bool generar = false)
        {
            var hoy = DateTime.Today;
            vm.Desde ??= hoy.AddDays(-29);
            vm.Hasta ??= hoy;

            if (generar)
            {
                var error = _svc.Validar(vm.Tipo, vm.Desde, vm.Hasta);
                if (error != null) ModelState.AddModelError("", error);
                else vm.Resultado = _svc.Generar(vm.Tipo, vm.Desde, vm.Hasta);
            }
            return View(vm);
        }

        public IActionResult Exportar(string? tipo, DateTime? desde, DateTime? hasta)
        {
            var error = _svc.Validar(tipo, desde, hasta);
            if (error != null)
            {
                TempData["Error"] = error;
                return RedirectToAction("Index");
            }

            var r = _svc.Generar(tipo!, desde, hasta);
            var nombre = $"informe_{tipo}_{DateTime.Now:yyyyMMdd_HHmm}.csv";
            return File(InformesService.ToCsv(r), "text/csv; charset=utf-8", nombre);
        }
    }
}