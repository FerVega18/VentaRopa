using System;
using System.Collections.Generic;

namespace Models;

public partial class Orden
{
    public int OrdenId { get; set; }

    public int? ClienteId { get; set; }

    public DateOnly? OrdenFecha { get; set; }

    public string? NombreD { get; set; }

    public string? DireccionD { get; set; }

    public int? EstadoId { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<DetallesOrden> DetallesOrdens { get; set; }

    public virtual EstadoOrden? Estado { get; set; }
}
