using System.Text;
using CarStockBLL.Interfaces;
using CarStockBLL.Middlewares;
using CarStockBLL.Models;
using CarStockBLL.Services;
using CarStockDAL.Data;
using CarStockDAL.Data.Repos;
using CarStockDAL.Models;
using CarStockMAP;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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


builder.Services.AddScoped<ICarRepository<Car>, PostgreCarRepository<Car>>();
builder.Services.AddScoped<IBrandRepository<Brand>, PostgreBrandRepository<Brand>>();
builder.Services.AddScoped<ICarModelRepository<CarModel>, PostgreCarModelRepository<CarModel>>();
builder.Services.AddScoped<IColorRepository<Color>, PostgreColorRepository<Color>>();
builder.Services.AddScoped<IUserRepository, PostgreUserRepository>();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<IColorService, ColorService>();


builder.Services.AddScoped<MapService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<ITokenService, TokenService>();
//builder.Services.AddSingleton<IConfiguration>(builder.Configuration);


// JWT
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

//аутентификация с JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtConfig = builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtConfig>>().Value;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret)),
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
