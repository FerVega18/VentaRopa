using System;
using System.Collections.Generic;

namespace Models;

public partial class DetallesOrden
{
    public int OrdenId { get; set; }

    public int ProductoId { get; set; }

    public decimal? Precio { get; set; }

    public int? Cantidad { get; set; }

    public virtual Orden Orden { get; set; } 

    public virtual Producto Producto { get; set; } 
}
