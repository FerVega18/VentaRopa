using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace VentaRopa.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly DbAa96f3VentaropaContext _dbContext;

        public CategoriasController(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Método para listar categorías
        public async Task<IActionResult> Index()
        {
            var categorias = await _dbContext.Categoria.ToListAsync();
            return View(categorias);
        }

        // Método para la vista de crear una nueva categoría
        public IActionResult Agregar()
        {
            return View();
        }

        // Método para la vista de editar una categoría existente
        public async Task<IActionResult> Editar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _dbContext.Categoria.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        // Método para manejar la edición de una categoría existente
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [Bind("CategoriaId,Descripcion")] Categoria categoria)
        {
            if (id != categoria.CategoriaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _dbContext.Update(categoria);
                    await _dbContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoriaExiste(categoria.CategoriaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

        // Método para confirmar la eliminación de una categoría
        public async Task<IActionResult> Eliminar(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _dbContext.Categoria
                .FirstOrDefaultAsync(m => m.CategoriaId == id);
            if (categoria == null)
            {
                return NotFound();
            }

            return View(categoria);
        }

        // Método para manejar la eliminación de una categoría
        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            var categoria = await _dbContext.Categoria.FindAsync(id);
            _dbContext.Categoria.Remove(categoria);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // Método para manejar la creación de una nueva categoría
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar([Bind("CategoriaId,Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Add(categoria);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(categoria);
        }

       

        private bool CategoriaExiste(int id)
        {
            return _dbContext.Categoria.Any(e => e.CategoriaId == id);
        }
    }
}
