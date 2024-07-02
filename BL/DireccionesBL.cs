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
    public class DireccionesBL
    {
        private DireccionesDA direccionesDA;

        public DireccionesBL(DbAa96f3VentaropaContext context)
        {
            direccionesDA = new DireccionesDA(context);

        }

        public int EditarDireccion(Direccion direccion, int clienteID)
        {
            try
            {


                return direccionesDA.EditarDireccion(direccion, clienteID);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public int EliminarDireccion(int clienteID)
        {
            try
            {
                return direccionesDA.EliminarDireccion(clienteID);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public Direccion obtenerDireccionPorCliente(int clienteID)
        {
            try
            {
                return direccionesDA.obtenerDireccionPorCliente(clienteID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int AgregarDireccion(Direccion direccion)
        {
            try
            {
                return direccionesDA.AgregarDireccion(direccion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
