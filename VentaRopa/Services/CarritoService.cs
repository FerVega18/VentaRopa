using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

public class CarritoService : ICarritoService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CarritoService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int ObtenerCantidadProductos()
    {
        var session = _httpContextAccessor.HttpContext.Session;
        var carrito = session.GetObjectFromJson<Dictionary<int, int>>("Carrito") ?? new Dictionary<int, int>();
        return carrito.Sum(item => item.Value);
    }
}
