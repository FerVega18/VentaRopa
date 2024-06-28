using BL;
using DA;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexi�n
var connectionString = "Data Source=SQL8006.site4now.net;Initial Catalog=db_aa96f3_ventaropa;User Id=db_aa96f3_ventaropa_admin;Password=A*nvt46Fe-;";

// Registrar el contexto de la base de datos
builder.Services.AddDbContext<DbAa96f3VentaropaContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar servicios de sesi�n
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n de la sesi�n
    options.Cookie.HttpOnly = true; // La cookie s�lo estar� disponible a trav�s de HTTP
    options.Cookie.IsEssential = true; // Es esencial para la aplicaci�n
});

// Registro de otros servicios
builder.Services.AddScoped<UsuarioBL>();
builder.Services.AddScoped<UsuarioDA>();
builder.Services.AddScoped<ProductosBL>();
builder.Services.AddScoped<CategoriasBL>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ClienteBL>();
builder.Services.AddScoped<TarjetasBL>();



// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// A�adir middleware de sesi�n
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Lista}/{id?}");

app.Run();

