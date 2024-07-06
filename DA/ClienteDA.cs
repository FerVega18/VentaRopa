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
                var rolCliente = _dbContext.Rols.FirstOrDefault(r => r.RolId == 1);
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

        public Cliente obtenerClientePorUsuario(string nombreUsuario)
        {
            try
            {
                Cliente cliente = _dbContext.Clientes
                    .Include(c => c.NombreUsuarioNavigation) // Incluir la navegación a Usuario
                    .Include(c => c.Direccions) // Incluir la colección de direcciones
                    .Include(c => c.Tarjeta) // Incluir la colección de tarjetas
                    .AsEnumerable()
                    .FirstOrDefault(c =>  c.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));

                if (cliente == null)
                {
                    throw new Exception($"Cliente con nombre de usuario '{nombreUsuario}' no encontrado.");
                }

                return cliente;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el cliente por nombre de usuario: " + ex.Message);
            }
        }

        public int AsociarDirecciones(Direccion direccion, Cliente cliente) {
            try { 
              cliente.Direccions.Add(direccion);
                _dbContext.SaveChanges();
                return cliente.Direccions.Count;
            } catch (Exception ex) {
                throw new Exception( ex.Message);
            }
        }


    }
}
