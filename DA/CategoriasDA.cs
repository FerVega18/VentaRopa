using Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DA
{
    public class CategoriasDA
    {
        private readonly DbAa96f3VentaropaContext _dbContext;

        public CategoriasDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Categoria>> ObtenerTodos()
        {
            try
            {
                return await _dbContext.Categoria.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todas las categorías: " + ex.Message);
            }
        }

        public Categoria ObtenerPorId(int id)
        {
            try
            {
                return _dbContext.Categoria.FirstOrDefault(c => c.CategoriaId == id);
                
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la categoría por ID: " + ex.Message);
            }
        }

        public async Task<int> Agregar(Categoria categoria)
        {
            try
            {
                await _dbContext.Categoria.AddAsync(categoria);
                await _dbContext.SaveChangesAsync();
                return categoria.CategoriaId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la categoría: " + ex.Message);
            }
        }

        public async Task<int> Actualizar(int id, Categoria categoria)
        {
            try
            {
                Categoria categoriaExistente =  ObtenerPorId(id);
                if (categoriaExistente != null)
                {
                    categoriaExistente.Descripcion = categoria.Descripcion;
                    _dbContext.Categoria.Update(categoriaExistente);
                    await _dbContext.SaveChangesAsync();
                    return categoriaExistente.CategoriaId;
                }
                else
                {
                    throw new Exception("Categoría no encontrada para actualizar");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la categoría: " + ex.Message);
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                var categoria = await _dbContext.Categoria.FirstOrDefaultAsync(c => c.CategoriaId == id);
                if (categoria != null)
                {
                    _dbContext.Categoria.Remove(categoria);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la categoría en el acceso a datos: " + ex.Message);
            }
        }
    }
}
