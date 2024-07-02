using BL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;

public class OrdenController : Controller
{
    private readonly DbAa96f3VentaropaContext _context;
    private readonly OrdenesBL _ordenesBL;
    private readonly DetallesOrdenBL _detallesOrdenBL;
    private readonly DireccionesBL _direccionesBL;
    private readonly ClienteBL _clienteBL;
    private readonly UsuarioBL _usuarioBL;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public OrdenController(OrdenesBL ordenesBL, DetallesOrdenBL detallesOrdenBL, IHttpContextAccessor httpContextAccessor, DireccionesBL direccionesBL, ClienteBL clienteBL, UsuarioBL usuarioBL, DbAa96f3VentaropaContext context)
    {
        _ordenesBL = ordenesBL;
        _detallesOrdenBL = detallesOrdenBL;
        _httpContextAccessor = httpContextAccessor;
        _direccionesBL = direccionesBL;
        _clienteBL = clienteBL;
        _usuarioBL = usuarioBL;
        _context = context;
    }

    [HttpGet]
    public IActionResult Compra()
    {
        if (User.Identity.IsAuthenticated)
        {
            var usuario = _usuarioBL.obtenerUsuarioPorNombre(User.Identity.Name);
            var cliente = _clienteBL.obtenerClientePorUsuario(usuario.NombreUsuario);

            ViewBag.Cliente = cliente;
            ViewBag.UsuarioAutenticado = true;
        }
        else
        {
            ViewBag.UsuarioAutenticado = false;
        }

        ViewBag.CompraProcesada = false; // Inicialmente, la compra no está procesada
        return View();
    }

    [HttpPost]
    public IActionResult ProcesarCompra(string nombre, string apellido, string direccion, string ciudad, string codigoPostal, string numeroTarjeta, string cvc, DateOnly fechaVencimiento, int? direccionId, int? tarjetaId, List<CarritoProducto> productos)
    {
        Orden orden = null;
        EstadoOrden estado = new EstadoOrden { Descripcion = "En proceso" };

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
                EstadoId = estado.EstadoId
            };
        }
        else
        {
            orden = new Orden
            {
                OrdenFecha = DateOnly.FromDateTime(DateTime.Now),
                NombreD = nombre,
            };
        }

        // Guardar orden y detalles de la orden
        _ordenesBL.Agregar(orden, orden.Estado);
        // Agregar detalles de la orden utilizando las cantidades proporcionadas
        foreach (var item in productos)
        {
            var producto = _context.Productos.FirstOrDefault(p => p.ProductoId == item.productoId);
            if (producto != null)
            {
                var detallesOrden = new DetallesOrden
                {
                    OrdenId = orden.OrdenId,
                    ProductoId = producto.ProductoId,
                    Cantidad = item.cantidad,
                    Precio = producto.Precio
                };

                _detallesOrdenBL.Agregar(detallesOrden);
            }
        }


        ViewBag.CompraProcesada = true;
        return RedirectToAction("Confirmacion");
    }
}







