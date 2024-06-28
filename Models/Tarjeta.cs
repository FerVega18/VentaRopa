using System;
using System.Collections.Generic;

namespace Models;

public partial class Tarjeta
{
    public int TarjetaId { get; set; }

    public int? ClienteId { get; set; }

    public string? Numero { get; set; }

    public string? Cvc { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual ICollection<TokenPago> TokenPagos { get; set; } = new List<TokenPago>();
}
