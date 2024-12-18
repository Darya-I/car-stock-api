using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using CarStockDAL.Data;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICarRepository<Car>, PostgreCarRepository<Car>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();