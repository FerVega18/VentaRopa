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
    public class DetallesOrdenBL
    {
        private DetallesOrdenDA detallesOrdenesDA;
        private OrdenesDA ordenesDA;

        public DetallesOrdenBL(DbAa96f3VentaropaContext context)
        {
            detallesOrdenesDA = new DetallesOrdenDA(context);
            ordenesDA = new OrdenesDA(context);
        }
        

        public int Agregar(DetallesOrden orden)
        {
            try
            {
                
                return detallesOrdenesDA.Agregar(orden);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DetallesOrden BuscarPorId(int numeroOrden)
        {
            try
            {
                return detallesOrdenesDA.BuscarPorId(numeroOrden);


            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los detalles de la orden por número de orden: " + ex.Message);
            }
        }


        public List<DetallesOrden> obtenerPorId(int ordenID)
        {
            try
            {
                return detallesOrdenesDA.obtenerPorId(ordenID);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public List<DetallesOrden> obtenerPorFecha(DateOnly fecha)
        {
            try
            {
                return detallesOrdenesDA.obtenerPorFecha(fecha);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);

            }
        }

        public List<DetallesOrden> obtenerPorCorreo(String nombre)
        {
            try
            {
                return detallesOrdenesDA.obtenerPorCorreo(nombre);

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
                return detallesOrdenesDA.obtenerPorProducto(producto);

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
                return detallesOrdenesDA.listarOrdenados();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
