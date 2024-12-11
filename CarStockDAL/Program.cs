using CarStockDAL.Data;
using CarStockDAL.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//      проверочка
builder.Services.AddScoped<PostgreCarRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//      проверочка

app.MapGet("test", (PostgreCarRepository repo) =>
{
    var newCar = new Car
    {
        BrandId = 2,
        ModelId = 2, 
        ColorId = 2  
    };

    repo.Create(newCar);
    repo.Save();

    var cars = repo.GetAllCars();

    return cars.Select(car => new
    {
        car.Id,
        car.BrandId,
        car.ModelId,
        car.ColorId
    });
});


app.UseHttpsRedirection();

app.Run();

