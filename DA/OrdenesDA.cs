using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class OrdenesDA
    {
        private readonly DbAa96f3VentaropaContext _dbContext;

        public OrdenesDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int Agregar(Orden orden) {
            try {
                _dbContext.Ordens.Add(orden);
                return orden.OrdenId;
            
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public List<Orden> ObtenerPorFecha(DateOnly fechaInicio, DateOnly fechaFin)
        {
            try
            {
                // Asegúrate de que la propiedad "Fecha" sea la correcta en tu entidad Orden
                return _dbContext.Set<Orden>()
                                 .Where(o => o.OrdenFecha >= fechaInicio && o.OrdenFecha <= fechaFin)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
