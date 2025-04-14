using Fsm.Application;
using Fsm.Adapters.Persistence;
using Fsm.WebApp; // Ensure this namespace is included


var builder = WebApplication.CreateBuilder(args);

// Servicios de la aplicación
builder.Services.AddApplication();  // Servicios de la aplicación
builder.Services.AddPersistence(builder.Configuration);// Servicios de la infraestructura
builder.Services.AddHelperServices(); // Servicio de notificaciones

builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
