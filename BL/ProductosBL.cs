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

        public async Task<List<Producto>> ObtenerTodos()
        {
            try
            {
                return await productosDA.ObtenerTodos();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        public  Producto obtenerPorId(int id)
        {
            try
            {
                return  productosDA.ObtenerPorId(id);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task Agregar(Producto producto)
        {
            try
            {
                await productosDA.Agregar(producto);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<int> Actualizar(int id, Producto producto)
        {

            try
            {

                return await productosDA.Actualizar(id, producto);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                await productosDA.Eliminar(id);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        public async Task<List<Producto>> BuscarPorNombre(string nombre)
        {
            try
            {
                return await productosDA.BuscarPorNombre(nombre);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        


        public async Task<List<Producto>> ObtenerTodosOrdenados(string orderBy)
        {
            try
            {
                var productos = await productosDA.ObtenerTodos(); // Esperar la tarea asincrónica

                switch (orderBy?.ToLower())
                {
                    case "precioasc":
                        return productos.OrderBy(p => p.Precio).ToList();
                    case "preciodesc":
                        return productos.OrderByDescending(p => p.Precio).ToList();
                    case "masreciente":
                        return productos.OrderByDescending(p => p.ProductoId).ToList(); // Suponiendo que ProductoID es un proxy de fecha de creación
                    case "maspopular":
                        var popularidad = await productosDA.ObtenerPopularidadProductos(); // Esperar la tarea asincrónica
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
