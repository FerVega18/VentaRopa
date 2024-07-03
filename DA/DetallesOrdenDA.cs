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

        public DetallesOrden obtenerPorId(int ordenID)
        {
            try {
                return _dbContext.DetallesOrdens.FirstOrDefault(d => d.OrdenId == ordenID);

            }
            catch (Exception ex) 
            {
                throw new Exception(ex.Message);
            
            }
        }

        public DetallesOrden obtenerPorFecha(DateOnly fecha)
        {
            try
            {
                return _dbContext.DetallesOrdens.FirstOrDefault(d => d.Orden.OrdenFecha == fecha);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public DetallesOrden obtenerPorCorreo(String nombre)
        {
            try
            {
                return _dbContext.DetallesOrdens.FirstOrDefault(d => d.Orden.Cliente.NombreUsuario == nombre);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

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

