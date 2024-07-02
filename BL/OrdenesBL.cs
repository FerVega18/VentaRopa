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
    public class OrdenesBL
    {

        private OrdenesDA ordenesDA;

        public OrdenesBL(DbAa96f3VentaropaContext context)
        {
            ordenesDA = new OrdenesDA(context);
        }

        public int Agregar(Orden orden, EstadoOrden estado)
        {
            try
            {
                orden.EstadoId = estado.EstadoId;
                orden.Estado = estado;
                return ordenesDA.Agregar(orden,estado);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Orden> ObtenerPorFecha(DateOnly fechaInicio, DateOnly fechaFin)
        {
            try
            {
                return ordenesDA.ObtenerPorFecha(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las órdenes por fecha: " + ex.Message);
            }
        }

       
    }
}
