using BL;
using DA;
using Microsoft.EntityFrameworkCore;
using Models;

var builder = WebApplication.CreateBuilder(args);

// Configurar la cadena de conexión
var connectionString = "Data Source=SQL8006.site4now.net;Initial Catalog=db_aa96f3_ventaropa;User Id=db_aa96f3_ventaropa_admin;Password=A*nvt46Fe-;";

// Registrar el contexto de la base de datos
builder.Services.AddDbContext<DbAa96f3VentaropaContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<UsuarioBL>();
builder.Services.AddScoped<UsuarioDA>();
// Registro de otros servicios
builder.Services.AddScoped<UsuarioBL>();

// Agregar controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
