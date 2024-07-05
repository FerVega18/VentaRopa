using BL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace VentaRopa.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioBL _usuarioBL;
        private readonly IHttpContextAccessor _httpContextAccessor;

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
                    return RedirectToAction("Crear", "Clientes");
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

        // GET: Usuarios/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Usuarios/Login
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
                    if (usuario.RolId != 1) // Verifica si el usuario tiene el rol de Cliente
                    {
                        ModelState.AddModelError(string.Empty, "Solo los clientes pueden iniciar sesión aquí.");
                        return View();
                    }

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                new Claim(ClaimTypes.Role, usuario.Rol.Descripcion) // Asegúrate de que el rol sea "Cliente"
            };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    return RedirectToAction("Lista", "Productos");
                }
                ModelState.AddModelError(string.Empty, "Nombre de usuario o contraseña incorrectos.");
            }
            return View();
        }




        [HttpGet("/admin")]
        [HttpGet("/ventas")]
        public IActionResult Login2(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost("/admin")]
        [HttpPost("/ventas")]
        public async Task<IActionResult> Login2(string nombreUsuario, string contraseña, string returnUrl)
        {
            // Validación manual
            var validationErrors = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(nombreUsuario))
            {
                validationErrors.Add("nombreUsuario", "El nombre de usuario es requerido.");
            }

            if (string.IsNullOrWhiteSpace(contraseña))
            {
                validationErrors.Add("contraseña", "La contraseña es requerida.");
            }

            if (validationErrors.Any())
            {
                ViewBag.ValidationErrors = validationErrors;
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var usuario = _usuarioBL.ObtenerUsuario(nombreUsuario, contraseña);
            if (usuario == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos.";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
        new Claim(ClaimTypes.Role, usuario.Rol.Descripcion)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            if (usuario.RolId == 3) // Si el rol es Administrador (id = 3)
            {
                return RedirectToAction("Gestionar", "Productos");
            }

            if (usuario.RolId == 2) // Si el rol es Ventas (id = 2)
            {
                return RedirectToAction("Index", "Orden");
            }

            if (usuario.RolId == 1) // Si el rol es Cliente (id = 1)
            {
                return RedirectToAction("Login", "Usuarios");
            }

            // Si llega aquí, el usuario no tiene un rol reconocido
            ViewBag.Error = "No tiene permisos para acceder a esta área.";
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Lista", "Productos"); 
        }

    }
}

