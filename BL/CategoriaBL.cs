using DA;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public List<Categoria> ObtenerTodos()
        {
            try
            {
                return categoriasDA.ObtenerTodos();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Categoria ObtenerPorId(int id)
        {
            try
            {
                return categoriasDA.ObtenerPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int Agregar(Categoria categoria)
        {
            try
            {
                return categoriasDA.Agregar(categoria);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int Actualizar(int id, Categoria categoria)
        {
            try
            {
                return categoriasDA.Actualizar(id, categoria);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                categoriasDA.Eliminar(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}