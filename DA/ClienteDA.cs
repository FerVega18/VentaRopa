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

        public Cliente obtenerClientePorUsuario(string nombreUsuario)
        {
            try
            {
                return _dbContext.Clientes.AsEnumerable().FirstOrDefault(u => u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



    }
}
