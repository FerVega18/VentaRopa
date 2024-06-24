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

        public DetallesOrdenBL(DbAa96f3VentaropaContext context)
        {
            detallesOrdenesDA = new DetallesOrdenDA(context);
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

        public DetallesOrden obtenerPorId(int ordenID)
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

        public DetallesOrden obtenerPorFecha(DateOnly fecha)
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

        public DetallesOrden obtenerPorCorreo(String nombre)
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
