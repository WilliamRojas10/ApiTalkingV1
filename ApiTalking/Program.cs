using DaoLibrary;
using Microsoft.EntityFrameworkCore;
using DaoLibrary.Interfaces;
using DaoLibrary.EFCore;

using DaoLibrary.Interfaces.User;
using DaoLibrary.EFCore.User;


var builder = WebApplication.CreateBuilder(args);

// Configura la conexión a la base de datos MySQL
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Registro de DAOFactory
builder.Services.AddScoped<IDAOFactory, DAOFactory>();
builder.Services.AddScoped<IDAOUser, DAOUser>(); // Registra DAOUser si es necesario

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del pipeline de solicitud HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
