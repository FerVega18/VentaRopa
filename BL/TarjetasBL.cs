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
    public class TarjetasBL
    {

        private TarjetasDA tarjetasDA;

        public TarjetasBL(DbAa96f3VentaropaContext context)
        {
            tarjetasDA = new TarjetasDA(context);
        }

        public int Agregar(Tarjetum tarjeta)
        {
            try
            {
                return tarjetasDA.Agregar(tarjeta);
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
                return tarjetasDA.AgregarToken(token);
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
                return tarjetasDA.EliminarToken(token);
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
                return tarjetasDA.EditarToken(token, id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
