using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.IO;
using System.Threading.Tasks;

namespace VentaRopa.Controllers
{
    public class ProductosController : Controller
    {
        private readonly DbAa96f3VentaropaContext _dbContext;

        public ProductosController(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Agregar()
        {
            ViewData["Categorias"] = await _dbContext.Categoria.ToListAsync();
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

                _dbContext.Productos.Add(producto);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Lista));
            }
            ViewData["Categorias"] = await _dbContext.Categoria.ToListAsync();
            return View(producto);
        }

        public async Task<IActionResult> Lista()
        {
            var productos = await _dbContext.Productos.ToListAsync();
            return View(productos);
        }

        // Método para mostrar detalles de un producto
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _dbContext.Productos
                .FirstOrDefaultAsync(m => m.ProductoId == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }



        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var producto = await _dbContext.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["Categorias"] = await _dbContext.Categoria.ToListAsync();
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

                    _dbContext.Update(producto);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExiste(producto.ProductoId))
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
            ViewData["Categorias"] = await _dbContext.Categoria.ToListAsync();
            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await _dbContext.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _dbContext.Productos.Remove(producto);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Gestionar));
        }

        private bool ProductoExiste(int id)
        {
            return _dbContext.Productos.Any(e => e.ProductoId == id);
        }

        public async Task<IActionResult> Gestionar()
        {
            var productos = await _dbContext.Productos.ToListAsync();
            return View(productos);
        }

    }
}
