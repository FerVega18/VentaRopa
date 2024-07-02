using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class DireccionesDA
    {
        private DbAa96f3VentaropaContext _dbContext;

        public DireccionesDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
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

    }
}
