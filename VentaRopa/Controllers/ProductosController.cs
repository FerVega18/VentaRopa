using BL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.IO;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace VentaRopa.Controllers
{

    public class ProductosController : Controller
    {

        private ProductosBL _productosBL;
        private CategoriasBL _categoriasBL;
        private IHttpContextAccessor _httpContextAccessor;
        private ClienteBL _clienteBL;
        private readonly ICarritoService _carritoService;

        public ProductosController(ProductosBL productosBL, CategoriasBL categoriasBL, IHttpContextAccessor httpContextAccessor, ClienteBL clienteBL, ICarritoService carritoService)
        {
            _productosBL = productosBL;
            _categoriasBL = categoriasBL;
            _httpContextAccessor = httpContextAccessor;
            _clienteBL = clienteBL;
            _carritoService = carritoService;
        }

        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Agregar()
        {
            ViewData["Categorias"] = await _categoriasBL.ObtenerTodos();
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Agregar([Bind("Descripcion,CategoriaId,Talla,Marca,Precio,Stock")] Producto producto, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imagenes");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }
                    producto.Imagen = "/Imagenes/" + uniqueFileName;
                    Categoria ca = _categoriasBL.ObtenerPorId(producto.CategoriaId.Value);
                    producto.Categoria = ca;
                }

                await _productosBL.Agregar(producto);
                return RedirectToAction(nameof(Lista));
            }
            ViewData["Categorias"] = await _categoriasBL.ObtenerTodos();
            return View(producto);
        }


        public async Task<IActionResult> Lista(string searchQuery, string sortOrder, string filter, int? categoria, string marca)
        {
            ViewBag.CarritoCantidad = _carritoService.ObtenerCantidadProductos();
            // Obtener todos los productos
            var productos = await _productosBL.ObtenerTodos();

            // Obtener todas las categorías y marcas
            ViewBag.Categorias = await _categoriasBL.ObtenerTodos();
            ViewBag.Marcas = productos.Select(p => p.Marca).Distinct().ToList();

            // Aplicar filtros de búsqueda
            if (!string.IsNullOrEmpty(filter))
            {
                switch (filter.ToLower())
                {
                    case "nombre":
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            searchQuery = searchQuery.ToLower();
                            productos = productos.Where(p => p.Descripcion.ToLower().Contains(searchQuery)).ToList();
                        }
                        break;

                    case "categoria":
                        if (categoria.HasValue)
                        {
                            productos = productos.Where(p => p.CategoriaId == categoria.Value).ToList();
                        }
                        break;

                    case "talla":
                        if (!string.IsNullOrEmpty(searchQuery))
                        {
                            searchQuery = searchQuery.ToLower();
                            productos = productos.Where(p => p.Talla.ToLower().Contains(searchQuery)).ToList();
                        }
                        break;

                    case "marca":
                        if (!string.IsNullOrEmpty(marca))
                        {
                            productos = productos.Where(p => p.Marca.ToLower() == marca.ToLower()).ToList();
                        }
                        break;
                }
            }

            // Ordenar los productos según el parámetro de orden
            switch (sortOrder)
            {
                case "price_asc":
                    productos = productos.OrderBy(p => p.Precio).ToList();
                    break;
                case "price_desc":
                    productos = productos.OrderByDescending(p => p.Precio).ToList();
                    break;
                case "recent":
                    productos = productos.OrderByDescending(p => p.ProductoId).ToList();
                    break;
                default:
                    productos = productos.OrderBy(p => p.Descripcion).ToList();
                    break;
            }

            return View(productos);
        }





        //Método para mostrar detalles de un producto
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.CarritoCantidad = _carritoService.ObtenerCantidadProductos();
            var producto =  _productosBL.obtenerPorId(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }


        [Authorize(Roles = "Administrador")]
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var producto =  _productosBL.obtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["Categorias"] = await _categoriasBL.ObtenerTodos();
            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Editar(int id, [Bind("ProductoId,Descripcion,CategoriaId,Talla,Marca,Precio,Stock,Imagen")] Producto producto, IFormFile imageFile)
        {
            if (id != producto.ProductoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imagenes");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }
                        producto.Imagen = "/Imagenes/" + uniqueFileName;
                    }

                    await _productosBL.Actualizar(id, producto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_productosBL.obtenerPorId(id) != null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Gestionar));
            }
            ViewData["Categorias"] = await _categoriasBL.ObtenerTodos();
            return View(producto);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto =  _productosBL.obtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }

            await _productosBL.Eliminar(id);
            return RedirectToAction(nameof(Gestionar));
        }

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Gestionar(string searchQuery)
        {
            try
            {
                var productos = await _productosBL.ObtenerTodos(); // Utiliza el método de ProductosBL para obtener todos los productos

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    searchQuery = searchQuery.ToLower(); // Convertir la consulta a minúsculas para comparación sin distinción de casos

                    productos = productos.Where(p =>
                        p.Descripcion.ToLower().Contains(searchQuery) ||
                        p.ProductoId.ToString().Contains(searchQuery) // No afectado por distinción de mayúsculas/minúsculas
                    ).ToList();
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al gestionar productos: " + ex.Message);
            }
        }


        [AllowAnonymous]
        public IActionResult AgregarAlCarrito(int productoId, int cantidad = 1)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var carrito = session.GetObjectFromJson<Dictionary<int, int>>("Carrito") ?? new Dictionary<int, int>();

            if (carrito.ContainsKey(productoId))
            {
                carrito[productoId] += cantidad;
            }
            else
            {
                carrito[productoId] = cantidad;
            }

            session.SetObjectAsJson("Carrito", carrito);

            return RedirectToAction("Lista");
        }



    }
}

    



