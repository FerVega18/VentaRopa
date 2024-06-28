using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class ClienteDA
    {

        private DbAa96f3VentaropaContext _dbContext;

        public ClienteDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Agregar(Cliente cliente)
        {
            try
            {

                _dbContext.Clientes.Add(cliente);
                _dbContext.SaveChanges();
                return cliente.ClienteId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Cliente ObtenerPorId(int id)
        {
            try
            {
                return _dbContext.Clientes.Include(c
                    => c.NombreUsuarioNavigation).FirstOrDefault(c // Incluir usuario asociado
                    => c.ClienteId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int Actualizar(Cliente cliente)
        {
            try
            {
                if (cliente != null)
                {
                    _dbContext.Clientes.Update(cliente);
                    _dbContext.SaveChanges();
                    return cliente.ClienteId;
                }
                else
                {
                    throw new Exception("Cliente vacio.");

                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        public int AgregarDirecciones(Direccion direccion, int clienteID)
        { //Implica una nueva tabla direcciones
            try
            {
                Cliente cliente = ObtenerPorId(clienteID);
                direccion.ClienteId = cliente.ClienteId;
                _dbContext.Direccions.Add(direccion);
                _dbContext.SaveChanges();
                return cliente.ClienteId;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int EditarDireccion(Direccion direccion, int clienteID)
        {
            try
            {


                Direccion direccionPorActualizar = obtenerDireccionPorCliente(clienteID);
                direccionPorActualizar.DireccionId = direccion.DireccionId;
                direccionPorActualizar.Descripcion = direccion.Descripcion;
                direccionPorActualizar.ClienteId = direccion.ClienteId;
                _dbContext.Direccions.Update(direccionPorActualizar);
                _dbContext.SaveChanges();
                return direccion.DireccionId;

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
                Direccion direccionPorEliminar = obtenerDireccionPorCliente(clienteID);
                _dbContext.Direccions.Remove(direccionPorEliminar);
                _dbContext.SaveChanges();
                return direccionPorEliminar.DireccionId;

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
                return _dbContext.Direccions.FirstOrDefault(d => d.ClienteId == clienteID);
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
                _dbContext.Direccions.Add(direccion);
                _dbContext.SaveChanges();
                return direccion.DireccionId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int EditarDireccion(Direccion direccion)
        {
            try
            {
                _dbContext.Direccions.Update(direccion);
                _dbContext.SaveChanges();
                return direccion.DireccionId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int EliminarDireccion(Direccion direccion)
        {
            try
            {
                _dbContext.Direccions.Remove(direccion);
                _dbContext.SaveChanges();
                return direccion.DireccionId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
