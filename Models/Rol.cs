using System;
using System.Collections.Generic;

namespace Models;

public partial class Rol
{
    public int RolId { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; }
}
