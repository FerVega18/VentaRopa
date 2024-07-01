using DA;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BL
{
    public class CategoriasBL
    {
        private CategoriasDA categoriasDA;

        public CategoriasBL(DbAa96f3VentaropaContext context)
        {
            categoriasDA = new CategoriasDA(context);
        }

        public async Task<List<Categoria>> ObtenerTodos()
        {
            try
            {
                return await categoriasDA.ObtenerTodos();
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
                Categoria ca = categoriasDA.ObtenerPorId(2);
                return ca;
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
                return await categoriasDA.Agregar(categoria);
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
                return await categoriasDA.Actualizar(id, categoria);
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
                await categoriasDA.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la categoría: " + ex.Message);
            }
        }
    }
}
