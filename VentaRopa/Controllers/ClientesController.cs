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
        private readonly TarjetasBL _tarjetasBL; 
        private DireccionesBL _direccionesBL;

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

                    // Agregar direcciones
                    foreach (var direccionDescripcion in direcciones)
                    {
                        var direccion = new Direccion { Descripcion = direccionDescripcion, ClienteId = clienteId };
                        _direccionesBL.AgregarDireccion(direccion, clienteId, true);
                    }

                    // Agregar tarjetas
                    for (int i = 0; i < tarjetas.Count; i++)
                    {
                        var tarjeta = new Tarjeta { Numero = tarjetas[i], Cvc = cvcTarjetas[i], FechaVencimiento = fechaVencimientoTarjetas[i], ClienteId = cliente.ClienteId };
                        _tarjetasBL.Agregar(tarjeta);
                    }

                    

                    _clienteBL.CrearCliente(cliente, contraseña, false);
                    return RedirectToAction("Lista", "Productos");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(string.Empty, "Error al intentar crear el cliente: " + ex.Message);
                }
            }
            return View();
        }


    }
}

