using Microsoft.EntityFrameworkCore;
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
                _dbContext.SaveChanges();

                return orden.OrdenId;
            
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public List<Orden> ObtenerPorFecha(DateOnly fechaInicio, DateOnly fechaFin)
        {
            try
            {
               
                return _dbContext.Set<Orden>()
                                 .Where(o => o.OrdenFecha >= fechaInicio && o.OrdenFecha <= fechaFin)
                                 .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task<EstadoOrden> obtenerEstadoPorID(int estado) {
            try
            {
                return await _dbContext.EstadoOrdens.FirstOrDefaultAsync(e => e.EstadoId == estado);

            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public Orden ObtenerPorNumero(int numeroOrden)
        {
            return _dbContext.Ordens.FirstOrDefault(o => o.OrdenId == numeroOrden);
        }

        public List<Orden> ObtenerPorCorreoUsuario(string correoUsuario)
        {
            try
            {
                return  _dbContext.Ordens.Where(o => o.Cliente.NombreUsuario == correoUsuario).ToList();
               
            }catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }   
            
        }

        public List<Orden> ObtenerPorNombreCliente(string nombreCliente)
        {
            try
            {
                return _dbContext.Ordens
                    .Include(o => o.Cliente)
                    .Where(o => o.Cliente.Nombre.Contains(nombreCliente) || o.Cliente.Apellido.Contains(nombreCliente))
                    .ToList();
            }catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Orden> ObtenerPorIdCliente(int clienteId)
        {
            try { 
                return _dbContext.Ordens.Where(o => o.ClienteId == clienteId).ToList();
            } catch(Exception ex)
            {
                throw new Exception(ex.Message + " " + clienteId);
            }
        }

        public List<Orden> ObtenerTodasOrdenes()
        {
            return _dbContext.Ordens.OrderBy(o => o.OrdenFecha).ToList();
        }

        public void Actualizar(Orden orden)
        {
            _dbContext.Entry(orden).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public Orden ObtenerPorId(int id)
        {
            try {
                return _dbContext.Ordens.FirstOrDefault(o => o.OrdenId == id);

            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public EstadoOrden obtenerEstado(int id) {
            try {
                return _dbContext.EstadoOrdens.FirstOrDefault(e => e.EstadoId == id);
            } catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

    }
}
