using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using Models;

namespace BL
{
    public class ProductosBL
    {
        private ProductosDA productosDA;

        public ProductosBL(DbAa96f3VentaropaContext context)
        {
            productosDA = new ProductosDA(context);
        }

        public List<Producto> ObtenerTodos()
        {
            try
            {
                return productosDA.ObtenerTodos();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        public Producto obtenerPorId(int id)
        {
            try
            {
                return productosDA.ObtenerPorId(id);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public void Agregar(Producto producto)
        {
            try
            {
                productosDA.Agregar(producto);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public int Actualizar(int id, Producto producto)
        {

            try
            {

                return productosDA.Actualizar(id, producto);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                productosDA.Eliminar(id);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public List<Producto> BuscarPorNombre(string nombre)
        {
            try
            {
                return productosDA.BuscarPorNombre(nombre);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        public List<Producto> BuscarPorCodigo(int codigo)
        {
            return productosDA.BuscarPorCodigo(codigo);
        }


        public List<Producto> ObtenerTodosOrdenados(string orderBy)
        {
            try
            {
                var productos = productosDA.ObtenerTodos();

                switch (orderBy?.ToLower())
                {
                    case "precioasc":
                        return productos.OrderBy(p => p.Precio).ToList();
                    case "preciodesc":
                        return productos.OrderByDescending(p => p.Precio).ToList();
                    case "masreciente":
                        return productos.OrderByDescending(p => p.ProductoId).ToList(); // Suponiendo que ProductoID es un proxy de fecha de creación
                    case "maspopular":
                        var popularidad = productosDA.ObtenerPopularidadProductos();
                        return productos.OrderByDescending(p => popularidad.ContainsKey(p.ProductoId) ? popularidad[p.ProductoId] : 0).ToList();
                    default:
                        return productos.ToList();
                }
            }
            catch (InvalidOperationException ex)
            {
                // Manejar errores específicos relacionados con operaciones inválidas
                throw new Exception("Ocurrió un error al ordenar los productos: " + ex.Message);
            }
            catch (Exception ex)
            {
                // Manejar otros tipos de errores genéricos
                throw new Exception("Ocurrió un error inesperado: " + ex.Message);
            }
        }



    }
}
