using Models;
using DA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public String Agregar(Usuario usuario)
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

        public Usuario ObtenerUsuario(string nombreUsuario, string contraseña)
        {
            try
            {
                return usuarioDA.ObtenerUsuario(nombreUsuario, contraseña);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<Usuario> ObtenerTodos()
        {
            try
            {
                return usuarioDA.ObtenerTodos();
            }
            catch (Exception ex)
            {

                throw new Exception("Error en UsuarioBL al obtener todos los usuarios: " + ex.Message);
            }
        }

    }
}
