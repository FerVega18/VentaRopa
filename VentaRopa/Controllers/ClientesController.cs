using BL;
using Microsoft.AspNetCore.Mvc;
using Models;
using System;
using System.Threading.Tasks;

namespace VentaRopa.Controllers
{
    public class ClientesController : Controller
    {
        private readonly ClienteBL _clienteBL;
        private readonly UsuarioBL _usuarioBL;
        private readonly TarjetasBL _tarjetasBL; // Agregamos la referencia a BL de Tarjetas

        public ClientesController(ClienteBL clienteBL, UsuarioBL usuarioBL, TarjetasBL tarjetasBL)
        {
            _clienteBL = clienteBL;
            _usuarioBL = usuarioBL;
            _tarjetasBL = tarjetasBL;
        }

        // GET: Clientes/Crear
        public IActionResult Crear()
        {
            return View();
        }

        // POST: Clientes/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(Cliente cliente, string contraseña)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar el usuario
                    var usuario = _usuarioBL.ObtenerUsuario(cliente.NombreUsuario, contraseña);

                    if (usuario == null)
                    {
                        ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                        return View(cliente);
                    }

                    // Asignar el usuario al cliente
                    cliente.NombreUsuarioNavigation = usuario;

                    // Crear el cliente
                    _clienteBL.CrearCliente(cliente, contraseña);

                    return RedirectToAction("Lista", "Productos");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error al intentar crear el cliente: " + ex.Message);
                }
            }
            return View(cliente);
        }
    }
}

