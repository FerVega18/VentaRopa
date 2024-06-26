using BL;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;

namespace VentaRopa.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioBL _usuarioBL;

        public UsuariosController(UsuarioBL usuarioBL)
        {
            _usuarioBL = usuarioBL;
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
        public IActionResult Index()
        {
            var usuarios = _usuarioBL.ObtenerTodos();
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
        public async Task<IActionResult> Login(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                var user = _usuarioBL.ObtenerUsuario(usuario.NombreUsuario, usuario.Contraseña);
                if (user != null)
                {
                    // Implementa aquí la lógica de autenticación (ejemplo: crear cookies de autenticación)
                    // await SignInUserAsync(user); // Ejemplo de método para autenticar al usuario
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }
            return View(usuario);
        }
    }
}

