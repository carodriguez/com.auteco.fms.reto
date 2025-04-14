using Fsm.Application;
using Fsm.Adapters.Persistence; // Ensure this namespace is included

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Servicios de la aplicación
builder.Services.AddApplication();
builder.Services.AddPersistence(builder.Configuration); 

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
