using CarStockDAL.Data;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();



// Регистрация репозиториев
builder.Services.AddScoped<ICarRepository<Car>, PostgreCarRepository<Car>>();
builder.Services.AddScoped<IBrandRepository<Brand>, PostgreBrandRepository<Brand>>();
builder.Services.AddScoped<ICarModelRepository<CarModel>, PostgreCarModelRepository<CarModel>>();
builder.Services.AddScoped<IColorRepository<Color>, PostgreColorRepository<Color>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();

