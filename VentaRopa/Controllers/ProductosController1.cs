using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
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
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar(Producto producto, IFormFile imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    // Define the path where the images will be stored
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Imagenes");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Ensure the directory exists
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Save the image to the specified path
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    // Save the relative path of the image in the database
                    producto.Imagen = "/Imagenes/" + uniqueFileName;
                }

                _dbContext.Productos.Add(producto);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");
            }
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
    }
}

