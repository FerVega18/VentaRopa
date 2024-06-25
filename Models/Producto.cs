using System;
using System.Collections.Generic;

namespace Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public int? CategoriaId { get; set; }

    public string? Descripcion { get; set; }

    public string? Talla { get; set; }

    public string? Marca { get; set; }

    public decimal? Precio { get; set; }

    public string? Imagen { get; set; }

    public int? Stock { get; set; }

    public virtual Categoria? Categoria { get; set; }

    public virtual ICollection<DetallesOrden> DetallesOrdens { get; set; } = new List<DetallesOrden>();
}
