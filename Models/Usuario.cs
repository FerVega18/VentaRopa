using System;
using System.Collections.Generic;

namespace Models;

public partial class Usuario
{
    public string NombreUsuario { get; set; } 

    public string? Contraseña { get; set; }

    public int? RolId { get; set; }

    public virtual ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();

    public virtual Rol? Rol { get; set; }
}
