using DA;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BL
{
    public class UsuarioBL
    {
        private UsuarioDA usuarioDA;

        public UsuarioBL(DbAa96f3VentaropaContext context)
        {
            usuarioDA = new UsuarioDA(context);
        }

        public string Agregar(Usuario usuario)
        {
            try
            {
                return usuarioDA.Agregar(usuario);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Usuario ObtenerUsuario(string nombreUsuario, string contrasena)
        {
            try
            {
                return  usuarioDA.ObtenerUsuario(nombreUsuario, contrasena);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Usuario>> ObtenerTodos()
        {
            try
            {
                 return await usuarioDA.ObtenerTodos();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
