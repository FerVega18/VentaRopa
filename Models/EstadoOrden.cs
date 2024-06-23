using System;
using System.Collections.Generic;

namespace Models;

public partial class EstadoOrden
{
    public int EstadoId { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Orden> Ordens { get; set; } = new List<Orden>();
}
