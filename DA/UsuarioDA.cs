using Models;

public class UsuarioDA
{
    private DbAa96f3VentaropaContext _dbContext;

    public UsuarioDA(DbAa96f3VentaropaContext dbContext)
    {
        _dbContext = dbContext;
    }

    public string Agregar(Usuario usuario)
    {
        try
        {
            if (_dbContext.Usuarios.Any(u => u.NombreUsuario == usuario.NombreUsuario))
            {
                throw new InvalidOperationException("El nombre de usuario ya está en uso.");
            }

            // Asignar el rol 'Cliente'
            var rolCliente = _dbContext.Rols.FirstOrDefault(r => r.Descripcion == "Cliente");
            if (rolCliente == null)
            {
                throw new Exception("El rol 'Cliente' no existe en la base de datos.");
            }
            usuario.RolId = rolCliente.RolId;

            _dbContext.Usuarios.Add(usuario);
            _dbContext.SaveChanges();
            return usuario.NombreUsuario;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public  Usuario ObtenerUsuario(string nombreUsuario, string contrasena)
    {
        try
        {
            Usuario usuario = _dbContext.Usuarios
                .AsEnumerable()
                .FirstOrDefault(u => u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase) && u.Contraseña == contrasena);
            return usuario;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public Usuario obtenerUsuarioPorNombre(string nombreUsuario) {
        try {
            return _dbContext.Usuarios.AsEnumerable().FirstOrDefault(u => u.NombreUsuario.Equals(nombreUsuario, StringComparison.OrdinalIgnoreCase));
          
        } catch (Exception ex) {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<Usuario>> ObtenerTodos()
    {
        try
        {
            return  _dbContext.Usuarios.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al obtener todos los usuarios: " + ex.Message);
        }
    }
}

