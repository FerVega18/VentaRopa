using Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;

namespace DA
{
    public class ProductosDA
    {
        private DbAa96f3VentaropaContext _dbContext;

        public ProductosDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Producto>> ObtenerTodos()
        {
            try
            {
                return await _dbContext.Productos.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los productos: " + ex.Message);
            }
        }

        public async Task<List<Producto>> ObtenerFiltrados(string filtro)
        {
            try
            {
                return await _dbContext.Productos.Where(filtro).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener productos filtrados: " + ex.Message);
            }
        }

        public async Task<Dictionary<int, int>> ObtenerPopularidadProductos()
        {
            try
            {
                var popularidad = new Dictionary<int, int>();

                var detallesOrden = await _dbContext.DetallesOrdens.ToListAsync();

                foreach (var detalle in detallesOrden)
                {
                    if (popularidad.ContainsKey(detalle.ProductoId))
                    {
                        popularidad[detalle.ProductoId] += detalle.Cantidad??0;
                    }
                    else
                    {
                        popularidad[detalle.ProductoId] = detalle.Cantidad??0;
                    }
                }

                return popularidad;
            }
            catch (Exception ex)
            {
                throw new Exception("Ocurrió un error al obtener la popularidad de productos: " + ex.Message);
            }
        }


        public Producto ObtenerPorId(int id)
        {
            try
            {
                return  _dbContext.Productos.FirstOrDefault(p => p.ProductoId == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el producto por ID: " + ex.Message);
            }
        }

        public async Task<int> Agregar(Producto producto)
        {
            try
            {
                _dbContext.Productos.Add(producto);
                await _dbContext.SaveChangesAsync();
                return producto.ProductoId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar el producto: " + ex.Message);
            }
        }

        public async Task<int> Actualizar(int id, Producto producto)
        {
            try
            {
                Producto productoExistente =  ObtenerPorId(id);
                if (productoExistente != null)
                {
                    productoExistente.CategoriaId = producto.CategoriaId;
                    productoExistente.Descripcion = producto.Descripcion;
                    productoExistente.Talla = producto.Talla;
                    productoExistente.Precio = producto.Precio;
                    productoExistente.Imagen = producto.Imagen;
                    productoExistente.Stock = producto.Stock;
                    productoExistente.Marca = producto.Marca;
                    await _dbContext.SaveChangesAsync();
                }
                return producto.ProductoId;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el producto: " + ex.Message);
            }
        }

        public async Task Eliminar(int id)
        {
            try
            {
                var producto = await _dbContext.Productos.FirstOrDefaultAsync(p => p.ProductoId == id);
                if (producto != null)
                {
                    _dbContext.Productos.Remove(producto);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el producto: " + ex.Message);
            }
        }

        public async Task<List<Producto>> BuscarPorNombre(string nombre)
        {
            try
            {
                return await _dbContext.Productos
                    .Where(p => p.Descripcion.ToLower().Contains(nombre.ToLower()))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar productos por nombre: " + ex.Message);
            }
        }

      

      



    }
}


