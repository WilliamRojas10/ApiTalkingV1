using DaoLibrary;
using Microsoft.EntityFrameworkCore;
using DaoLibrary.Interfaces;
using DaoLibrary.EFCore;
using Microsoft.OpenApi.Models;
using DaoLibrary.Interfaces.User;
using DaoLibrary.EFCore.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiTalking.Service;
using DaoLibrary.Interfaces.Course;
using DaoLibrary.EFCore.Course;
using DaoLibrary.EFCore.Post;
using DaoLibrary.Interfaces.Post;
using DaoLibrary.EFCore.Reaction;
using DaoLibrary.Interfaces.Reaction;
using Microsoft.Extensions.FileProviders;
using System.IO;
using DaoLibrary.EFCore.File;
using DaoLibrary.Interfaces.File;
using DaoLibrary.Interfaces.Comment;
using DaoLibrary.EFCore.Comment;

var builder = WebApplication.CreateBuilder(args);

// 🔹 1️⃣ Configurar la conexión a MySQL
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// 🔹 2️⃣ Registro de DAOFactory
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IDAOFactory, DAOFactory>();
builder.Services.AddScoped<IDAOUser, DAOUser>(); // Registra DAOUser si es necesario
builder.Services.AddScoped<IDAOCourse, DAOCourse>();
builder.Services.AddScoped<IDAOPost, DAOPost>();
builder.Services.AddScoped<IDAOReaction, DAOReaction>();
builder.Services.AddScoped<IDAOFile, DAOFile>();
builder.Services.AddScoped<IDAOComment, DAOComment>();

// Registrar FileService
builder.Services.AddScoped<FileService>();
// 🔹 3️⃣ Agregar Autenticación y Autorización JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

builder.Services.AddAuthorization();

// 🔹 4️⃣ Habilitar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// 🔹 5️⃣ Configurar Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ApiTalking", Version = "v1" });

    // Mapea IFormFile para que Swagger lo reconozca como un archivo binario
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });

    // 🔹 Habilitar Autenticación JWT en Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Introduce el token JWT en el siguiente formato: Bearer {tu_token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();

// 🔹 6️⃣ Configuración del pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// 🔹 Agregar configuración para servir archivos desde "FilesSystem"
var filesPath = Path.Combine(Directory.GetCurrentDirectory(), "FilesSystem");
if (!Directory.Exists(filesPath))
{
    Directory.CreateDirectory(filesPath);
}
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "FilesSystem")),
    RequestPath = "/FilesSystem"
});

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(
//        Path.Combine(Directory.GetCurrentDirectory(), "FilesSystem", "Images", "Posts")),
//    RequestPath = "/FilesSystem/Images/Posts"
//});


// 🔹 7️⃣ Aplicar los Middlewares en el orden correcto
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
