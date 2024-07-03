using BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models;
using Newtonsoft.Json;
using System.Linq;

//--Ya tiene todas las validaciones--
public class OrdenController : Controller
{
    private readonly OrdenesBL _ordenesBL;
    private readonly DetallesOrdenBL _detallesOrdenBL;
    private readonly DireccionesBL _direccionesBL;
    private readonly ClienteBL _clienteBL;
    private readonly UsuarioBL _usuarioBL;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private TarjetasBL _tarjetasBL;
    private ProductosBL _productosBL;
    public OrdenController(OrdenesBL ordenesBL, DetallesOrdenBL detallesOrdenBL, IHttpContextAccessor httpContextAccessor, DireccionesBL direccionesBL, ClienteBL clienteBL, UsuarioBL usuarioBL, TarjetasBL tarjetasBL, ProductosBL productosBL)
    {
        _ordenesBL = ordenesBL;
        _detallesOrdenBL = detallesOrdenBL;
        _httpContextAccessor = httpContextAccessor;
        _direccionesBL = direccionesBL;
        _clienteBL = clienteBL;
        _usuarioBL = usuarioBL;
        _tarjetasBL = tarjetasBL;
        _productosBL = productosBL;
    }

    //Accion para verificar si el usuario está logeado antes de realizar la compra
    public IActionResult Compra(string productos)
    {
        var productosList = JsonConvert.DeserializeObject<List<CarritoProducto>>(productos);
        ViewBag.Productos = productosList;

        var usuarioAutenticado = User.Identity.IsAuthenticated;
        Cliente cliente = null;

        if (usuarioAutenticado)
        {
            string usuario = User.Identity.Name;
            cliente = _clienteBL.obtenerClientePorUsuario(usuario);
        }

        ViewBag.UsuarioAutenticado = usuarioAutenticado;
        ViewBag.Cliente = cliente;
        ViewBag.CompraProcesada = false;

        return View(productosList);
    }

    [HttpPost]
    public IActionResult ProcesarCompra(string nombre, string apellido, string direccion, string numeroTarjeta, string cvc, DateOnly fechaVencimiento, int cedula, int? tarjetaId, List<CarritoProducto> productos)
    {
        Orden orden = null;

        if (User.Identity.IsAuthenticated)
        {
            var usuario = _usuarioBL.obtenerUsuarioPorNombre(User.Identity.Name);
            var cliente = _clienteBL.obtenerClientePorUsuario(usuario.NombreUsuario);

            var direccionEntrega = _direccionesBL.obtenerDireccionPorCliente(cliente.ClienteId);
            var tarjeta = cliente.Tarjeta.FirstOrDefault(t => t.TarjetaId == tarjetaId);

            orden = new Orden
            {
                ClienteId = cliente.ClienteId,
                OrdenFecha = DateOnly.FromDateTime(DateTime.Now),
                NombreD = nombre,
                DireccionD = direccionEntrega.Descripcion,
                EstadoId = 3,
            };
        }
        else
        {
            Cliente clienteExistente = _clienteBL.ObtenerPorId(cedula);
            if (clienteExistente != null)
            {
                TempData["AlertaClienteRegistrado"] = "Parece que ya tienes una cuenta. ¡Inicia sesión!";
                return View("Compra",productos);

            }
            else
            {
                Cliente cliente = new Cliente
                {
                    ClienteId = cedula,
                    Nombre = nombre,
                    Apellido = apellido,
                };
                _clienteBL.CrearCliente(cliente, "", true);

                Direccion direccionCliente = new Direccion
                {
                    ClienteId = cedula,
                    Descripcion = direccion,
                };
                _direccionesBL.AgregarDireccion(direccionCliente, cedula);
                orden = new Orden
                {

                    ClienteId = cedula,
                    OrdenFecha = DateOnly.FromDateTime(DateTime.Now),
                    NombreD = nombre,
                    DireccionD = direccion,
                    EstadoId = 3,
                };

                Tarjeta tarjetaCliente = new Tarjeta
                {
                    Numero = numeroTarjeta,
                    Cvc = cvc,
                    FechaVencimiento = fechaVencimiento,
                };
                _tarjetasBL.Agregar(tarjetaCliente);
                _ordenesBL.Agregar(orden);
            }
            

        }

        // Guardar orden y detalles de la orden

        foreach (var item in productos)
        {
            Producto producto = _productosBL.obtenerPorId(item.productoId);
            if (producto != null)
            {
                var detallesOrden = new DetallesOrden
                {
                    
                    ProductoId = producto.ProductoId,
                    Cantidad = item.cantidad,
                    Precio = producto.Precio
                };

                _detallesOrdenBL.Agregar(detallesOrden,orden.OrdenId);
            }
        }

        ViewBag.CompraProcesada = true;
        return View("Compra");
    }
}








