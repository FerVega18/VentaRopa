using System;
using System.Collections.Generic;

namespace Models;

public partial class TokenPago
{
    public int TokenId { get; set; }

    public int? TarjetaId { get; set; }

    public string? Descripcion { get; set; }

    public virtual Tarjeta? Tarjeta { get; set; }
}
