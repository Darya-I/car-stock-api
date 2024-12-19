using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using CarStockDAL.Data.Repos;
using CarStockDAL.Data;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;
using CarStockMAP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// Регистрация MapService
builder.Services.AddScoped<MapService>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
