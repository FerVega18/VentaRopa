﻿using BL;
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nombreUsuario, string contraseña)
        {
            if (ModelState.IsValid)
            {
                var usuario = _usuarioBL.ObtenerUsuario(nombreUsuario, contraseña);
                
                if (usuario != null)
                {
                    // Crear las claims del usuario
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                        new Claim(ClaimTypes.Role, usuario.Rol.ToString()) 
                    };

                    // Crear la identidad del usuario
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Crear las propiedades de autenticación
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    // Iniciar sesión con cookies
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

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
        public async Task<IActionResult> Logout()
        {
            // Cerrar sesión con cookies
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Lista", "Productos");
        }



    }
}

