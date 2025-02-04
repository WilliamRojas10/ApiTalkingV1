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

var builder = WebApplication.CreateBuilder(args);

// 🔹 1️⃣ Configurar la conexión a MySQL
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));

// 🔹 2️⃣ Registro de DAOFactory
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IDAOFactory, DAOFactory>();
builder.Services.AddScoped<IDAOUser, DAOUser>(); // Registra DAOUser si es necesario

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

// 🔹 7️⃣ Aplicar los Middlewares en el orden correcto
app.UseCors("AllowAllOrigins"); // Habilitar CORS antes de la autenticación
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
