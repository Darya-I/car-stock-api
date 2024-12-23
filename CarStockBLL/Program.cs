using CarStockBLL;
using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using CarStockDAL.Data;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Регистрация сервисов
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<IColorService, ColorService>();

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.Run();
