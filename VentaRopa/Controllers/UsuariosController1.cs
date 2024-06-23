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

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("NombreUsuario,Contraseña,RolId")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _usuarioBL.Agregar(usuario);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Ocurrió un error al crear el usuario: " + ex.Message);
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
