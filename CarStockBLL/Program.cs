
using CarStockBLL.Interfaces;
using CarStockBLL.Services;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();



// Регистрация сервисов
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<IColorService, ColorService>();

builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var app = builder.Build();


app.UseHttpsRedirection();



app.Run();
