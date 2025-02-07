using DaoLibrary;
using Microsoft.EntityFrameworkCore;
using DaoLibrary.Interfaces;
using DaoLibrary.EFCore;
using Microsoft.OpenApi.Models; // Para OpenApiSchema y OpenApiInfo

using DaoLibrary.Interfaces.User;
using DaoLibrary.EFCore.User;
using DaoLibrary.Interfaces.Course;
using DaoLibrary.EFCore.Course;
using DaoLibrary.EFCore.Post;
using DaoLibrary.Interfaces.Post;







var builder = WebApplication.CreateBuilder(args);

// Configura la conexión a la base de datos MySQL
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// Registro de DAOFactory
builder.Services.AddScoped<IDAOFactory, DAOFactory>();
builder.Services.AddScoped<IDAOUser, DAOUser>(); // Registra DAOUser si es necesario
builder.Services.AddScoped<IDAOCourse, DAOCourse>();
builder.Services.AddScoped<IDAOPost, DAOPost>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiTalking", Version = "v1" });

    // Mapea IFormFile para que Swagger lo reconozca como un tipo de archivo binario
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});


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
app.UseStaticFiles();
app.Run();
