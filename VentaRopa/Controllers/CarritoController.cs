using Microsoft.AspNetCore.Mvc;

namespace VentaRopa.Controllers
{
    

        public class CarritoController : Controller
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            public CarritoController(IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
            }

            public IActionResult Agregar(int productoId)
            {
                var session = _httpContextAccessor.HttpContext.Session;
                var carrito = session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
                carrito.Add(productoId);
                session.SetObjectAsJson("Carrito", carrito);

                return RedirectToAction("Index", "Home"); // Redirige a la página de inicio o a donde prefieras
            }

            public IActionResult Ver()
            {
                var session = _httpContextAccessor.HttpContext.Session;
                var carrito = session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
                return View(carrito);
            }
        }
}
