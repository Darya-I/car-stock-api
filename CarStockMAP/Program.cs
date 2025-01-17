using CarStockDAL.Data;
using Microsoft.EntityFrameworkCore;
using CarStockMAP;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

// Регистрация MapService
builder.Services.AddScoped<UserMapService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.Run();
