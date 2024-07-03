using BL;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

public class CarritoController : Controller
{
    private readonly ProductosBL _productosBL;
    private readonly ClienteBL _clienteBL;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CarritoController(ProductosBL productosBL, ClienteBL clienteBL, IHttpContextAccessor httpContextAccessor)
    {
        _productosBL = productosBL;
        _clienteBL = clienteBL;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<IActionResult> Index()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var carrito = session.GetObjectFromJson<Dictionary<int, int>>("Carrito") ?? new Dictionary<int, int>();

        if (carrito.Count == 0)
        {
            TempData["CarritoVacio"] = "Tu carrito de compras está vacío.";
        }

        var productosCarrito = new List<(Producto producto, int cantidad)>();

        foreach (var item in carrito)
        {
            var producto = _productosBL.obtenerPorId(item.Key);
            if (producto != null)
            {
                productosCarrito.Add((producto, item.Value));
            }
        }

        return View(productosCarrito);
    }

    [HttpPost]
    public IActionResult Eliminar(int productoId)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var carrito = session.GetObjectFromJson<Dictionary<int, int>>("Carrito") ?? new Dictionary<int, int>();

        if (carrito.ContainsKey(productoId))
        {
            carrito.Remove(productoId);
            session.SetObjectAsJson("Carrito", carrito);
        }

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> Actualizar(List<CarritoProducto> productos)
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var carrito = session.GetObjectFromJson<Dictionary<int, int>>("Carrito") ?? new Dictionary<int, int>();

        foreach (var item in productos)
        {
            var producto = _productosBL.obtenerPorId(item.productoId);
            if (producto == null)
            {
                continue;
            }

            if (item.cantidad > producto.Stock)
            {
                TempData["CantidadExcedida"] = $"No hay suficiente stock disponible para {producto.Descripcion}. Stock disponible: {producto.Stock}.";
                return RedirectToAction("Index");
            }

            if (item.cantidad > 0)
            {
                carrito[item.productoId] = item.cantidad;
            }
            else
            {
                carrito.Remove(item.productoId);
            }
        }

        session.SetObjectAsJson("Carrito", carrito);

        return RedirectToAction("Index");
    }


    [HttpPost]
    public async Task<IActionResult> Compra(List<CarritoProducto> productos)
    {
        if (productos == null || productos.Count == 0)
        {
            TempData["CarritoVacio"] = "Tu carrito de compras está vacío.";
            return RedirectToAction("Index");
        }

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

        // Pasar los productos como parámetro al controlador Ordenes
        return RedirectToAction("Compra", "Orden", new { productos = JsonConvert.SerializeObject(productos) });
    }
}
