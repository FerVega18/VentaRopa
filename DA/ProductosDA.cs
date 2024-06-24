using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace DA
{
    public class ProductosDA
    {
        private DbAa96f3VentaropaContext _dbContext;

        public ProductosDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Producto> ObtenerTodos()
        {
            try
            {
                return _dbContext.Productos.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Producto> ObtenerFiltrados(string filtro)
        {
            try
            {
                return _dbContext.Productos.Where(filtro).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Dictionary<int, int> ObtenerPopularidadProductos()
        {
            try
            {
                var popularidad = new Dictionary<int, int>();

                var detallesOrden = _dbContext.DetallesOrdens.ToList();

                foreach (var detalle in detallesOrden)
                {
                    if (popularidad.ContainsKey(detalle.ProductoId))
                    {

                        if (detalle.Cantidad.HasValue)//Verificar si detalle.Canntidad tiene valor
                        {
                            //Obtenemos ese valor entero y lo sumamos o asignamos.
                            popularidad[detalle.ProductoId] += detalle.Cantidad.Value;
                        }
                    }
                    else
                    {
                        // Asignar la cantidad solo si detalle.Cantidad tiene un valor
                        if (detalle.Cantidad.HasValue)
                        {
                            popularidad[detalle.ProductoId] = detalle.Cantidad.Value;
                        }
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
                return _dbContext.Productos.FirstOrDefault(p => p.ProductoId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int Agregar(Producto producto)
        {
            try
            {
                _dbContext.Productos.Add(producto);
                _dbContext.SaveChanges();
                return producto.ProductoId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int Actualizar(int id, Producto producto)
        {
            try
            {
                Producto productoExistente = ObtenerPorId(id);
                productoExistente.CategoriaId = producto.CategoriaId;
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.Talla = producto.Talla;
                productoExistente.Precio = producto.Precio;
                productoExistente.Imagen = producto.Imagen;
                productoExistente.Stock = producto.Stock;
                _dbContext.Productos.Update(producto);
                _dbContext.SaveChanges();
                return producto.ProductoId;
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
                var producto = _dbContext.Productos.FirstOrDefault(p => p.ProductoId == id);
                if (producto != null)
                {
                    _dbContext.Productos.Remove(producto);
                    _dbContext.SaveChanges();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public List<Producto> BuscarPorNombre(string Nombre)
        {
            try
            {
                return _dbContext.Productos.Where(p => p.Descripcion.Contains(Nombre)).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<Producto> BuscarPorCodigo(int codigo)
        {
            try
            {
                return _dbContext.Productos.Where(p => p.ProductoId == codigo).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
