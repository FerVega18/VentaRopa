using BL;
using DA;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión
var connectionString = "Data Source=SQL8006.site4now.net;Initial Catalog=db_aa96f3_ventaropa;User Id=db_aa96f3_ventaropa_admin;Password=A*nvt46Fe-;";

// Registrar el contexto de la base de datos
builder.Services.AddDbContext<DbAa96f3VentaropaContext>(options =>
    options.UseSqlServer(connectionString));

// Registrar servicios de sesión
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true; // La cookie sólo estará disponible a través de HTTP
    options.Cookie.IsEssential = true; // Es esencial para la aplicación
});

// Registrar servicios
builder.Services.AddScoped<UsuarioBL>();
builder.Services.AddScoped<UsuarioDA>();
builder.Services.AddScoped<ProductosBL>();
builder.Services.AddScoped<CategoriasBL>();
builder.Services.AddScoped<ClienteBL>();
builder.Services.AddScoped<DireccionesDA>();
builder.Services.AddScoped<DireccionesBL>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<TarjetasBL>();
builder.Services.AddScoped<OrdenesBL>();
builder.Services.AddScoped<DetallesOrdenBL>();
builder.Services.AddScoped<DetallesOrdenDA>();


// Configurar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Expiración de la cookie de autenticación
        options.SlidingExpiration = true; // Renovar la cookie en cada solicitud
        options.Cookie.HttpOnly = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrador"));
    options.AddPolicy("RequireSalesRole", policy => policy.RequireRole("Ventas"));
    options.AddPolicy("RequireClientRole", policy => policy.RequireRole("Cliente"));
});

// Configurar la expiración de la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


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


// Añadir middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

// Añadir middleware de sesión
app.UseSession();

app.MapControllerRoute(
    name: "admin",
    pattern: "admin",
    defaults: new { controller = "Usuarios", action = "Login2" });

app.MapControllerRoute(
    name: "ventas",
    pattern: "ventas",
    defaults: new { controller = "Usuarios", action = "Login2" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Productos}/{action=Lista}/{id?}");

app.Run();
