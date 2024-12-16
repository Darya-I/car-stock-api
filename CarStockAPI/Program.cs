using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using CarStockDAL.Data;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );


builder.Services.AddScoped<ICarRepository<Car>, PostgreCarRepository<Car>>();

builder.Services.AddScoped<ICarService, CarService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();


app.Run();

