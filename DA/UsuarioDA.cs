﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA
{
    public class UsuarioDA
    {

        private readonly DbAa96f3VentaropaContext _dbContext;

        public UsuarioDA(DbAa96f3VentaropaContext dbContext)
        {
            _dbContext = dbContext;
        }


        public string Agregar(Usuario usuario)
        {
            try
            {
                // Validar que el nombre de usuario no esté repetido
                if (_dbContext.Usuarios.Any(u => u.NombreUsuario == usuario.NombreUsuario))
                {
                    throw new InvalidOperationException("El nombre de usuario ya está en uso.");
                }

                _dbContext.Usuarios.Add(usuario);
                _dbContext.SaveChanges();
                return usuario.NombreUsuario;
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
                return _dbContext.Usuarios
                .FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Contraseña == contrasena);
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
                return _dbContext.Usuarios.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todos los usuarios: " + ex.Message);
            }
        }
    }
}
