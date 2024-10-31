//AGREGADO POR GPT
using DaoLibrary;
using DaoLibrary.Interfaces;
using Microsoft.EntityFrameworkCore;

using DaoLibrary.EFCore;
//FIN

var builder = WebApplication.CreateBuilder(args);

// AGREGADO POR GPT - Add services to the container.
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    new MySqlServerVersion(new Version(8, 0, 21))));
    
//FIN
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IDAOFactory, DAOFactory>();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();