using System;
using System.Text;
using CarStockBLL;
using CarStockBLL.Interfaces;
using CarStockBLL.Middlewares;
using CarStockBLL.Services;
using CarStockDAL.Data;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using CarStockMAP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );


//                          !!! NEED TO REFACTOR !!! SERVICES REGISTER
builder.Services.AddScoped<ICarRepository<Car>, PostgreCarRepository<Car>>();
builder.Services.AddScoped<IBrandRepository<Brand>, PostgreBrandRepository<Brand>>();
builder.Services.AddScoped<ICarModelRepository<CarModel>, PostgreCarModelRepository<CarModel>>();
builder.Services.AddScoped<IColorRepository<Color>, PostgreColorRepository<Color>>();
builder.Services.AddScoped<IUserRepository, PostgreUserRepository>();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<IColorService, ColorService>();

// еще регистрация
builder.Services.AddScoped<MapService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<ITokenService, TokenService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


// JWT
var jwtConfig = builder.Configuration.GetSection("JwtConfig");
var secretKey = jwtConfig.GetValue<string>("Secret");

if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("JWT Secret Key is not configured.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig.GetValue<string>("Issuer"),
        ValidAudience = jwtConfig.GetValue<string>("Audience"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", policy =>
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser());
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandling>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
