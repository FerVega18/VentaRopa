﻿using DA;
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

        public int EliminarDireccion(int direccion)
        {
            try
            {
                return direccionesDA.EliminarDireccion(direccion);
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

        public Direccion ObtenerDireccionPorId(int direccionID)
        {
            try
            {
                return direccionesDA.ObtenerDireccionPorId(direccionID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public List<Direccion> obtenerDireccionesPorCliente(int clienteId)
        {
            try
            {
                return direccionesDA.obtenerDireccionesPorCliente(clienteId);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int AgregarDireccion(Direccion direccion, int cliente)
        {
            try
            {
            
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
