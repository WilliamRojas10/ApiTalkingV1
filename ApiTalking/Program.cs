using DaoLibrary;
using DaoLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;
using DaoLibrary.EFCore;

var builder = WebApplication.CreateBuilder(args);

// Configuración del DbContext para usar MySQL
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Agregar servicios al contenedor
builder.Services.AddControllers();
builder.Services.AddScoped<IDAOFactory, DAOFactory>();

// Configuración de Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
