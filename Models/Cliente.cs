using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models;

public partial class Cliente
{
    
    public int ClienteId { get; set; }
   
    public string? NombreUsuario { get; set; }
    
    public string? Nombre { get; set; }
    
    public string? Apellido { get; set; }
  
    public DateOnly? Nacimiento { get; set; }
    
    public string? Pais { get; set; }

    public virtual ICollection<Direccion> Direccions { get; set; } 

    public virtual Usuario? NombreUsuarioNavigation { get; set; }

    public virtual ICollection<Orden> Ordens { get; set; } 

    public virtual ICollection<Tarjeta> Tarjeta { get; set; } 
}
