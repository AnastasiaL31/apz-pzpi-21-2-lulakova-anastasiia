global using Microsoft.EntityFrameworkCore;
global using SmartShelter_WebAPI.Data;
global using SmartShelter_WebAPI.Interfaces;
global using SmartShelter_WebAPI.Models;
global using SmartShelter_WebAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<SmartShelterDBContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"))
    );
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
