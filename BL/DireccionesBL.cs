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
        private ClienteDA clienteDA;

        public DireccionesBL(DbAa96f3VentaropaContext context)
        {
            direccionesDA = new DireccionesDA(context);
            clienteDA = new ClienteDA(context) ;
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

        public int AgregarDireccion(Direccion direccion, int cliente, bool clienteNuevo)
        {
            try
            {
                if (clienteNuevo) {
                    return direccionesDA.AgregarDireccion(direccion);
                }
            
               direccion.ClienteId = clienteDA.ObtenerPorId(cliente).ClienteId;
            return direccionesDA.AgregarDireccion(direccion);

        }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void ActualizarDireccion(Direccion direccion)
        {
            try {
                direccionesDA.Actualizar(direccion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
