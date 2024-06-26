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

        public async Task<IActionResult> Lista()
        {
            var productos = await _productosBL.ObtenerTodos();
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

        public IActionResult AgregarAlCarrito(int productoId)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var carrito = session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
            carrito.Add(productoId);
            session.SetObjectAsJson("Carrito", carrito);

            return RedirectToAction("Lista"); // Redirige a la página de inicio o a donde prefieras
        }

        public IActionResult Ver()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var carrito = session.GetObjectFromJson<List<int>>("Carrito") ?? new List<int>();
            return View(carrito);
        }
    }

}

