using Microsoft.AspNetCore.Mvc;

namespace MVC_Contactos.Controllers
{
    public class PrincipalController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult ayuda() => View();
        public IActionResult Acerca() => View();
        public IActionResult Contacto() => View();
    }
}