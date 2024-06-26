using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using BL;

namespace VentaRopa.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly CategoriasBL _categoriasBL;

        public CategoriasController(CategoriasBL categoriasBL)
        {
            _categoriasBL = categoriasBL;
        }

        public async Task<IActionResult> Index()
        {
            var categorias = await _categoriasBL.ObtenerTodos();
            return View(categorias);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Agregar([Bind("Descripcion")] Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriasBL.Agregar(categoria);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al agregar la categoría: " + ex.Message);
                }
            }
            return View(categoria);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var categoria = await _categoriasBL.ObtenerPorId(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
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
                    await _categoriasBL.Actualizar(id, categoria);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error al editar la categoría: " + ex.Message);
                }
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Eliminar(int id)
        {
            try
            {
                await _categoriasBL.Eliminar(id);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error al eliminar la categoría: " + ex.Message);
                return RedirectToAction(nameof(Index));
            }
        }

    }
}
