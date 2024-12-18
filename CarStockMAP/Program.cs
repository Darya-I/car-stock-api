using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using CarStockDAL.Data.Repos;
using CarStockDAL.Data;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

builder.Services.AddScoped<ICarRepository<Car>, PostgreCarRepository<Car>>();

builder.Services.AddScoped<ICarService, CarService>();
//builder.Services.AddScoped<IBrandService, BrandService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
