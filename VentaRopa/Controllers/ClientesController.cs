using Microsoft.AspNetCore.Mvc;

namespace VentaRopa.Controllers
{
    public class ClientesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
