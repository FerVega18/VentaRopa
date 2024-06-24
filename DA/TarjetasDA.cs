using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class TarjetasDA
    {

        private readonly DbAa96f3VentaropaContext _dbContext;

        public TarjetasDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Agregar(Tarjetum tarjeta)
        {
            try
            {
                _dbContext.Tarjeta.Add(tarjeta);
                _dbContext.SaveChanges();
                return tarjeta.TarjetaId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public TokenPago ObtenerTokenPorId(int id)
        {
            try
            {
                return _dbContext.TokenPagos.FirstOrDefault(c => c.TokenId == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int AgregarToken(TokenPago token)
        {
            try
            {
                _dbContext.TokenPagos.Add(token);
                _dbContext.SaveChanges();
                return token.TokenId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int EliminarToken(TokenPago token)
        {
            try
            {
                _dbContext.TokenPagos.Remove(token);
                return token.TokenId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int EditarToken(TokenPago token, int id)
        {
            try
            {
                // Obtener el token existente de la base de datos
                TokenPago tokenExistente = ObtenerTokenPorId(id);

                // Verificar si el token existe
                if (tokenExistente == null)
                {
                    throw new Exception("El token no existe.");
                }

                // Actualizar las propiedades del token existente con las nuevas
                tokenExistente.Descripcion = token.Descripcion;
                tokenExistente.TarjetaId = token.TarjetaId; // Actualizar otras propiedades si es necesario

                // Marcar el token existente como modificado
                _dbContext.TokenPagos.Update(tokenExistente);

                // Guardar los cambios en la base de datos
                _dbContext.SaveChanges();

                // Devolver el ID del token actualizado
                return tokenExistente.TokenId;
            }
            catch (Exception ex)
            {
                // Manejar la excepción y lanzar una nueva
                throw new Exception(ex.Message);
            }
        }

    }
}
