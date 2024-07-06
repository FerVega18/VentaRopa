using BL;
using Microsoft.AspNetCore.Authorization;
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
        private readonly TarjetasBL _tarjetasBL; 
        private DireccionesBL _direccionesBL;
        private static bool clienteNuevo = false; //Variable estática para saber si el cliente que se va a crear es nuevo

        public ClientesController(ClienteBL clienteBL, UsuarioBL usuarioBL, TarjetasBL tarjetasBL,DireccionesBL direccionesBL)
        {
            _clienteBL = clienteBL;
            _usuarioBL = usuarioBL;
            _tarjetasBL = tarjetasBL;
            _direccionesBL = direccionesBL;
        }

        // GET: Clientes/Crear
        public IActionResult Crear()
        {
            clienteNuevo = true; //Indicamos que es un cliente nuevo
            return View();
        }

        // POST: Clientes/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Crear(int clienteId, string nombre, string apellido, DateOnly? nacimiento, string pais, string nombreUsuario, string contraseña, List<string> direcciones, List<string> tarjetas, List<string> cvcTarjetas, List<DateOnly> fechaVencimientoTarjetas)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar el usuario
                    var usuario = _usuarioBL.ObtenerUsuario(nombreUsuario, contraseña);
                    if (usuario == null)
                    {
                        ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
                        return View();
                    }

                    if (_clienteBL.ObtenerPorId(clienteId) != null)
                    {
                        ModelState.AddModelError(string.Empty, "El cliente ya existe.");
                        return View();
                    }
                   
                    
                    // Crear el objeto Cliente
                    var cliente = new Cliente
                    {
                        ClienteId = clienteId,
                        Nombre = nombre,
                        Apellido = apellido,
                        Nacimiento = nacimiento,
                        Pais = pais,
                        NombreUsuario = nombreUsuario,
                        NombreUsuarioNavigation = usuario
                    };
                    _clienteBL.CrearCliente(cliente, contraseña, false);
                    //if (_clienteBL.obtenerClientePorUsuario(usuario.NombreUsuario) != null)
                    //{
                    //    ModelState.AddModelError(string.Empty, "Ya existe un cliente asociado a ese correo");
                    //    return View();
                    //}

                    // Agregar direcciones
                    foreach (var direccionDescripcion in direcciones)
                    {
                        var direccion = new Direccion { Descripcion = direccionDescripcion, ClienteId = clienteId, Cliente = cliente };
                        _direccionesBL.AgregarDireccion(direccion, clienteId);
                    }

                    // Agregar tarjetas
                    for (int i = 0; i < tarjetas.Count; i++)
                    {
                        var tarjeta = new Tarjeta { Numero = tarjetas[i], Cvc = cvcTarjetas[i], FechaVencimiento = fechaVencimientoTarjetas[i], ClienteId = cliente.ClienteId };
                        _tarjetasBL.Agregar(tarjeta,cliente);
                    }

                    
                    return RedirectToAction("Lista", "Productos");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error al intentar crear el cliente: " + ex.Message);
                }
            }
            return View();
        }



        public IActionResult MisDirecciones()
        {
            var nombreUsuario = User.Identity.Name;
            var cliente = _clienteBL.obtenerClientePorUsuario(nombreUsuario); 
            if (cliente == null)
            {
                return NotFound();
            }
            var direccion = _direccionesBL.obtenerDireccionPorCliente(cliente.ClienteId);
            return View(direccion);
        }

        public IActionResult AgregarDireccion()
        {
            return View(new Direccion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarDireccion(Direccion direccion)
        {
            var nombreUsuario = User.Identity.Name;
            var cliente = _clienteBL.obtenerClientePorUsuario(nombreUsuario);
            if (cliente == null)
            {
                return NotFound();
            }

            try
            {
                _direccionesBL.AgregarDireccion(direccion, cliente.ClienteId);
                return RedirectToAction("MisDirecciones");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al agregar la dirección: {ex.Message}");
                return View(direccion);
            }
        }

        public IActionResult EditarDireccion(int clienteId)
        {
            var direccion = _direccionesBL.obtenerDireccionPorCliente(clienteId);
            return View(direccion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarDireccion(Direccion direccion)
        {
            try
            {
                _direccionesBL.ActualizarDireccion(direccion);
                return RedirectToAction("MisDirecciones");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al actualizar la dirección: {ex.Message}");
                return View(direccion);
            }
        }

        public IActionResult EliminarDireccion(int clienteId)
        {
            try
            {
                _direccionesBL.EliminarDireccion(clienteId);
                return RedirectToAction("MisDirecciones");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error al eliminar la dirección: {ex.Message}");
                var direccion = _direccionesBL.obtenerDireccionPorCliente(clienteId);
                return View("MisDirecciones", direccion);
            }
        }
    }
    

}

