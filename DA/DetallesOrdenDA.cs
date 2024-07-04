using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DetallesOrdenDA
    {
        private readonly DbAa96f3VentaropaContext _dbContext;

        public DetallesOrdenDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Agregar(DetallesOrden orden)
        {
            try { 
              _dbContext.DetallesOrdens.Add(orden);
                _dbContext.SaveChanges();
                return orden.OrdenId;
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public List<DetallesOrden> obtenerPorId(int numeroOrden)
        {
            try
            {
                return _dbContext.DetallesOrdens
                                 .Where(d => d.Orden.OrdenId == numeroOrden)
                                 .Include(d => d.Producto)
                                 .Include(d => d.Orden)
                                     .ThenInclude(o => o.Cliente)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de la orden por número de orden: " + ex.Message);
            }
        }

        public DetallesOrden BuscarPorId(int numeroOrden)
        {
            try
            {
                return _dbContext.DetallesOrdens
                                 .Include(d => d.Orden) // Incluir la propiedad de navegación Orden
                                 .FirstOrDefault(d => d.Orden.OrdenId == numeroOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de la orden por número de orden: " + ex.Message);
            }
        }


        public List<DetallesOrden> obtenerPorFecha(DateOnly fecha)
        {
            try
            {
                return _dbContext.DetallesOrdens
                                 .Where(d => d.Orden.OrdenFecha == fecha)
                                 .Include(d => d.Producto)
                                 .Include(d => d.Orden)
                                     .ThenInclude(o => o.Cliente)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de la orden por fecha: " + ex.Message);
            }
        }

        public List<DetallesOrden> obtenerPorCorreo(string correoUsuario)
        {
            try
            {
                return _dbContext.DetallesOrdens
                                 .Where(d => d.Orden.Cliente.NombreUsuario == correoUsuario)
                                 .Include(d => d.Producto)
                                 .Include(d => d.Orden)
                                     .ThenInclude(o => o.Cliente)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de la orden por correo de usuario: " + ex.Message);
            }
        }


        public DetallesOrden obtenerPorProducto(String producto)
        {
            try
            {
                return _dbContext.DetallesOrdens.FirstOrDefault(d => d.Producto.Descripcion == producto);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public List<DetallesOrden> listarOrdenados()
        {
            try
            {
                return _dbContext.Set<DetallesOrden>()
                                 .Include(d => d.Producto) // Incluir Producto
                                 .Include(d => d.Orden)    // Incluir Orden
                                    .ThenInclude(o => o.Cliente) // Incluir Cliente de la Orden
                                 .OrderBy(o => o.Orden.OrdenFecha)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}

