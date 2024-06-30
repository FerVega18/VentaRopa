using BL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.IO;
using System.Threading.Tasks;

namespace VentaRopa.Controllers
{
    public class ProductosController : Controller
    {
        private ProductosBL _productosBL;
        private CategoriasBL _categoriasBL;
        private  IHttpContextAccessor _httpContextAccessor;


        public ProductosController(ProductosBL productosBL, CategoriasBL categoriasBL, IHttpContextAccessor  httpContextAccessor)
        {
            _productosBL = productosBL;
            _categoriasBL = categoriasBL;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<IActionResult> Agregar()
        {
            ViewData["Categorias"] = await _categoriasBL.ObtenerTodos();
            return View();
        }

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
                }

                await _productosBL.Agregar(producto);
                return RedirectToAction(nameof(Lista));
            }
            ViewData["Categorias"] = await _categoriasBL.ObtenerTodos();
            return View(producto);
        }

        public async Task<IActionResult> Lista(string searchQuery, string sortOrder)
        {
            var productos = await _productosBL.ObtenerTodos();

            // Filtrar los productos por la consulta de búsqueda
            if (!string.IsNullOrEmpty(searchQuery))
            {
                searchQuery = searchQuery.ToLower(); // Convertir la consulta a minúsculas para comparación sin distinción de casos

                productos = productos.Where(p =>
                    p.Talla.ToLower().Contains(searchQuery) ||
                    p.CategoriaId.ToString().Contains(searchQuery) ||
                    p.Marca.ToString().Contains(searchQuery) ||
                    p.Descripcion.ToString().Contains(searchQuery)// No afectado por distinción de mayúsculas/minúsculas
                ).ToList();
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

            // Pasar los valores a la vista a través de ViewData
            ViewData["searchQuery"] = searchQuery;
            ViewData["sortOrder"] = sortOrder;

            return View(productos);
        }


        //Método para mostrar detalles de un producto
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _productosBL.obtenerPorId(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }



        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _productosBL.obtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["Categorias"] =  await _categoriasBL.ObtenerTodos();
            return View(producto);
        }

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

                    await _productosBL.Actualizar(id,producto);
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
            ViewData["Categorias"] = await  _categoriasBL.ObtenerTodos();
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _productosBL.obtenerPorId(id);
            if (producto == null)
            {
                return NotFound();
            }

            await _productosBL.Eliminar(id);
            return RedirectToAction(nameof(Gestionar));
        }


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


        public async Task<IActionResult> Carrito()
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
                var producto = await _productosBL.obtenerPorId(item.Key);
                if (producto != null)
                {
                    productosCarrito.Add((producto, item.Value));
                }
            }

            return View(productosCarrito);
        }

        public async Task<IActionResult> Buscar(string searchQuery) {
            try
            {
                var productos = await _productosBL.ObtenerTodos(); // Utiliza el método de ProductosBL para obtener todos los productos

                if (!string.IsNullOrEmpty(searchQuery))
                {
                    searchQuery = searchQuery.ToLower(); // Convertir la consulta a minúsculas para comparación sin distinción de casos

                    productos = productos.Where(p =>
                        p.Talla.ToLower().Contains(searchQuery) ||
                        p.CategoriaId.ToString().Contains(searchQuery) ||
                        p.Marca.ToString().Contains(searchQuery) ||
                        p.Descripcion.ToString().Contains(searchQuery)// No afectado por distinción de mayúsculas/minúsculas
                    ).ToList();
                }

                return View(productos);
            }
            catch (Exception ex)
            {
                return BadRequest("Error al gestionar productos: " + ex.Message);
            }

        }


        [HttpPost]
        public IActionResult EliminarDelCarrito(int productoId)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var carrito = session.GetObjectFromJson<Dictionary<int, int>>("Carrito") ?? new Dictionary<int, int>();

            if (carrito.ContainsKey(productoId))
            {
                carrito.Remove(productoId);
                session.SetObjectAsJson("Carrito", carrito);
            }

            return RedirectToAction("Carrito");
        }

        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> ActualizarCarrito(List<CarritoProducto> productos)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var carrito = session.GetObjectFromJson<Dictionary<int, int>>("Carrito") ?? new Dictionary<int, int>();

            foreach (var item in productos)
            {
                var producto = await _productosBL.obtenerPorId(item.productoId);
                if (producto == null)
                {
                    continue;
                }

                if (item.cantidad > producto.Stock)
                {
                    TempData["CantidadExcedida"] = $"No hay suficiente stock disponible para {producto.Descripcion}. Stock disponible: {producto.Stock}.";
                    return RedirectToAction("Carrito");
                }

                if (carrito.ContainsKey(item.productoId))
                {
                    if (item.cantidad > 0)
                    {
                        carrito[item.productoId] = item.cantidad;
                    }
                    else
                    {
                        carrito.Remove(item.productoId);
                    }
                }
            }

            session.SetObjectAsJson("Carrito", carrito);

            return RedirectToAction("Carrito");
        }

        public class CarritoProducto
        {
            public int productoId { get; set; }
            public int cantidad { get; set; }
        }



        [HttpPost]
        public IActionResult Comprar()
        {
            
           
            // Si el usuario no ha iniciado sesión, redirigirlo a la página de inicio de sesión
            return RedirectToAction("Crear","Clientes");
            

            // Aquí puedes implementar la lógica para procesar la compra

            //return RedirectToAction("ConfirmacionCompra");
        }

        

    }
}

    



