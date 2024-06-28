using BL;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace VentaRopa.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioBL _usuarioBL;
        private IHttpContextAccessor _httpContextAccessor;

        public UsuariosController(UsuarioBL usuarioBL, IHttpContextAccessor httpContextAccessor)
        {
            _usuarioBL = usuarioBL;
            _httpContextAccessor = httpContextAccessor;
        }

        // GET: Usuarios/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Usuarios/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _usuarioBL.Agregar(usuario);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error al intentar registrar el usuario: " + ex.Message);
                }
            }
            return View(usuario);
        }

        // GET: Usuario/Index
        public async Task<IActionResult> Index()
        {
            var usuarios = await _usuarioBL.ObtenerTodos();
            return View(usuarios);
        }

        // GET: Usuarios/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nombreUsuario, string contraseña)
        {
            if (ModelState.IsValid)
            {
                var usuario = _usuarioBL.ObtenerUsuario(nombreUsuario, contraseña);
                if (usuario != null)
                {
                    HttpContext.Session.SetString("Usuario", usuario.NombreUsuario); // Guarda el nombre del usuario en la sesión

                    // Mover productos del carrito no autenticado al autenticado, si existen
                    var session = _httpContextAccessor.HttpContext.Session;
                    var carrito = session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
                    if (carrito.Count > 0)
                    {
                        // Asocia el carrito con el usuario autenticado (ejemplo conceptual)
                        // Guardar carrito en la base de datos o asociarlo con el usuario
                    }

                    return RedirectToAction("Lista", "Productos");
                }
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }
            return View();
        }

        // GET: Usuarios/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Limpia la sesión
            return RedirectToAction("Lista", "Productos");
        }
    }
}
