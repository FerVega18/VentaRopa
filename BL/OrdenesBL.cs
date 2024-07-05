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

        public int Agregar(Orden orden)
        {
            try
            {
                
                return ordenesDA.Agregar(orden);
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

        public Orden ObtenerPorNumero(int numeroOrden)
        {
            try
            {
                return ordenesDA.ObtenerPorNumero(numeroOrden);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la orden por número: " + ex.Message);
            }
        }

        public List<Orden> ObtenerPorCorreoUsuario(string correoUsuario)
        {
            try
            {
                List<Orden> lista = ordenesDA.ObtenerPorCorreoUsuario(correoUsuario);
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las órdenes por correo de usuario: " + ex.Message);
            }
        }

        public List<Orden> ObtenerPorNombreCliente(string nombreCliente)
        {
            try
            {
                return ordenesDA.ObtenerPorNombreCliente(nombreCliente);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las órdenes por nombre de cliente: " + ex.Message);
            }
        }

        public List<Orden> ObtenerPorIdCliente(int clienteId)
        {
            try
            {
                return ordenesDA.ObtenerPorIdCliente(clienteId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener las órdenes por ID de cliente: " + ex.Message);
            }
        }

        public List<Orden> ObtenerTodasOrdenes()
        {
            try
            {
                return ordenesDA.ObtenerTodasOrdenes();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todas las órdenes: " + ex.Message);
            }
        }


        public void Actualizar(Orden orden)
        {
            try {
                ordenesDA.Actualizar(orden);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public Orden ObtenerPorId(int id)
        {
            try
            {
                return ordenesDA.ObtenerPorId(id);
            } catch(Exception ex) { 
                throw new Exception(ex.Message);
            }
            
        }

    }
}
