using BL;
using DA;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Models;
using Newtonsoft.Json;
using System.Linq;


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
        if (cedula.ToString().Length > 9)
        {
            ModelState.AddModelError("cedula", "La cédula no puede tener más de 9 dígitos.");
            return View("Compra", productos);
        }

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
                return View("Compra", productos);
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
                _direccionesBL.AgregarDireccion(direccionCliente, cedula, true);

                orden = new Orden
                {
                    ClienteId = cedula,
                    Cliente = cliente,
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
                    Producto = producto,
                    OrdenId = orden.OrdenId,
                    Orden = orden,
                    Cantidad = item.cantidad,
                    Precio = producto.Precio
                };
                _detallesOrdenBL.Agregar(detallesOrden);
            }
        }

        ViewBag.CompraProcesada = true;
        return View("Compra");
    }




    [Authorize(Roles = "Ventas")]
    public async Task<IActionResult> Index()
    {
        var ordenesSinDespachar = _detallesOrdenBL.listarOrdenados().Where(o => o.Orden.EstadoId == 3).ToList();
        var ordenesDespachadas = _detallesOrdenBL.listarOrdenados().Where(o => o.Orden.EstadoId == 4).ToList();

        var model = new Tuple<IEnumerable<DetallesOrden>, IEnumerable<DetallesOrden>>(ordenesSinDespachar, ordenesDespachadas);
        return View(model);
    }


    [Authorize(Roles = "Ventas")]
    [HttpGet]
    public IActionResult Buscar(string criterioBusqueda, int? numeroOrden, string correoUsuario, DateOnly? fechaInicio, DateOnly? fechaFin, string nombreCliente, int? clienteId)
    {
        List<DetallesOrden> ordenesSinDespachar = new List<DetallesOrden>();
        List<DetallesOrden> ordenesDespachadas = new List<DetallesOrden>();

        if (criterioBusqueda == "numeroOrden" && numeroOrden.HasValue)
        {
            ordenesSinDespachar = _detallesOrdenBL.obtenerPorId(numeroOrden.Value).Where(o => o.Orden.EstadoId == 3).ToList();
            ordenesDespachadas = _detallesOrdenBL.obtenerPorId(numeroOrden.Value).Where(o => o.Orden.EstadoId == 4).ToList();
        }
        else if (criterioBusqueda == "correoUsuario" && !string.IsNullOrEmpty(correoUsuario))
        {
            ordenesSinDespachar = _detallesOrdenBL.obtenerPorCorreo(correoUsuario).Where(o => o.Orden.EstadoId == 3).ToList();
            ordenesDespachadas = _detallesOrdenBL.obtenerPorCorreo(correoUsuario).Where(o => o.Orden.EstadoId == 4).ToList();
        }
        else if (criterioBusqueda == "fecha" && fechaInicio.HasValue && fechaFin.HasValue)
        {
            ordenesSinDespachar = _detallesOrdenBL.obtenerPorRangoFechas(fechaInicio.Value, fechaFin.Value).Where(o => o.Orden.EstadoId == 3).ToList();
            ordenesDespachadas = _detallesOrdenBL.obtenerPorRangoFechas(fechaInicio.Value, fechaFin.Value).Where(o => o.Orden.EstadoId == 4).ToList();
        }
        else if (criterioBusqueda == "nombreCliente" && !string.IsNullOrEmpty(nombreCliente))
        {
            ordenesSinDespachar = _detallesOrdenBL.obtenerPorNombreCliente(nombreCliente).Where(o => o.Orden.EstadoId == 3).ToList();
            ordenesDespachadas = _detallesOrdenBL.obtenerPorNombreCliente(nombreCliente).Where(o => o.Orden.EstadoId == 4).ToList();
        }
        else if (criterioBusqueda == "clienteId" && clienteId.HasValue)
        {
            ordenesSinDespachar = _detallesOrdenBL.obtenerPorClienteId(clienteId.Value).Where(o => o.Orden.EstadoId == 3).ToList();
            ordenesDespachadas = _detallesOrdenBL.obtenerPorClienteId(clienteId.Value).Where(o => o.Orden.EstadoId == 4).ToList();
        }

        var model = new Tuple<IEnumerable<DetallesOrden>, IEnumerable<DetallesOrden>>(ordenesSinDespachar, ordenesDespachadas);
        return View("Index", model);
    }

    [Authorize(Roles = "Ventas")]
    [HttpPost]
    public IActionResult Despachar(int ordenId)
    {
        var orden = _ordenesBL.ObtenerPorId(ordenId);

        if (orden != null)
        {
            orden.EstadoId = 4; // Cambiar el estado a "Despachada"
            _ordenesBL.Actualizar(orden);

            TempData["MensajeDespacho"] = $"Orden {ordenId} despachada correctamente.";
        }
        else
        {
            TempData["MensajeDespacho"] = $"Error al despachar la orden {ordenId}.";
        }

        return RedirectToAction("Index");
    }

}








