using System;
using System.Collections.Generic;

namespace Models;

public partial class Direccion
{
    public int DireccionId { get; set; }

    public string Descripcion { get; set; } = null!;

    public int ClienteId { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;
}
