using BL;
using Microsoft.AspNetCore.Mvc;
using Models;


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
                catch (InvalidOperationException ex) // Capturar la excepción específica
                {
                    ModelState.AddModelError(string.Empty, ex.Message); // Agregar mensaje de error a ModelState
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
    }
}
