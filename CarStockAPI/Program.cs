using System.Text;
using CarStockAPI.Middlewares;
using CarStockBLL.Interfaces;
using CarStockBLL.Services;
using CarStockDAL.Data;
using CarStockDAL.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NpgsqlTypes;
using Serilog;
using Serilog.Sinks.PostgreSQL;
using Microsoft.AspNetCore.Authentication.Cookies;
using CarStockDAL.Data.Interfaces;
using CarStockDAL.Data.Repositories;
using CarStockAPI.Configs;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

//                                                  настройки serilog
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true, reloadOnChange: true);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration) 
        .ReadFrom.Services(services)
        .WriteTo.PostgreSQL(
            connectionString: context.Configuration.GetConnectionString("DefaultConnection"),
            tableName: "Logs",
            needAutoCreateTable: true,
            columnOptions: new Dictionary<string, ColumnWriterBase>
            {
                { "message", new RenderedMessageColumnWriter() },
                { "message_template", new MessageTemplateColumnWriter() },
                { "level", new LevelColumnWriter(true, NpgsqlDbType.Varchar) },
                { "raise_date", new TimestampColumnWriter() },
                { "exception", new ExceptionColumnWriter() },
                { "properties", new LogEventSerializedColumnWriter() }
            }); ;
});

builder.Services.AddEndpointsApiExplorer();

// Настройка в сваггере для авторизации
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition(name: JwtBearerDefaults.AuthenticationScheme,
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Description = "Enter the Bearer authorization : `Bearer Generated-JWT-Token`",
            In = ParameterLocation.Header,
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer"
        });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
        new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = JwtBearerDefaults.AuthenticationScheme
            }
        },

        new string[] {}
        }
    });
}
);

builder.Services.AddControllers();


builder.Services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//                                              Регистрация сервисов
builder.Services.AddScoped<ICarRepository<Car>, PostgreCarRepository<Car>>();
builder.Services.AddScoped<IBrandRepository<Brand>, PostgreBrandRepository<Brand>>();
builder.Services.AddScoped<ICarModelRepository<CarModel>, PostgreCarModelRepository<CarModel>>();
builder.Services.AddScoped<IColorRepository<Color>, PostgreColorRepository<Color>>();
builder.Services.AddScoped<IUserRepository, PostgreUserRepository>();

builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IBrandService, BrandService>();
builder.Services.AddScoped<ICarModelService, CarModelService>();
builder.Services.AddScoped<IColorService, ColorService>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthorizeUserService, AuthorizeUserService>();

var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();

builder.Services.AddScoped<ITokenService, TokenService>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<ITokenService>>();
    return new TokenService(jwtConfig.Secret, jwtConfig.Issuer, jwtConfig.Audience, logger);
});

// Google
builder.Services.Configure<GoogleConfig>(builder.Configuration.GetSection("Authentication:Google"));

// Аутентификация с JWT
builder.Services.AddAuthentication((options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }))
    .AddJwtBearer(options =>
            {
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
            })
        .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
        {
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            options.SlidingExpiration = true;
        }) // Добавление схемы Cookie
        .AddGoogle(options =>
        {
            var googleConfig = builder.Configuration.GetSection("Authentication:Google").Get<GoogleConfig>();
            options.ClientId = googleConfig.ClientId;
            options.ClientSecret = googleConfig.ClientSecret;
            options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme; // Использование Cookie для Google
        });

// Настройки политик
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Bearer", policy =>
        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
              .RequireAuthenticatedUser());
    
    options.AddPolicy("CreateCarPolicy", policy =>
       policy.RequireClaim("Permission", "CanCreateCar"));
    
    options.AddPolicy("EditCarPolicy", policy =>
        policy.RequireClaim("Permission", "CanEditCar"));
    
    options.AddPolicy("DeleteCarPolicy", policy =>
        policy.RequireClaim("Permission", "CanDeleteCar"));
    
    options.AddPolicy("ViewCarPolicy", policy =>
        policy.RequireClaim("Permission", "CanViewCar"));

    options.AddPolicy("CreateUserPolicy", policy =>
        policy.RequireClaim("Permission", "CanCreateUser"));
    
    options.AddPolicy("EditUserPolicy", policy =>
            policy.RequireClaim("Permission", "CanEditUser"));
    
    options.AddPolicy("DeleteUserPolicy", policy =>
            policy.RequireClaim("Permission", "CanDeleteUser"));
    
    options.AddPolicy("ViewUserPolicy", policy =>
            policy.RequireClaim("Permission", "CanViewUser"));
});

// для откладки пока не используется чтоб потрогать гугл
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
var allowedHeaders = builder.Configuration.GetSection("Cors:AllowedHeaders").Get<string[]>();
var allowedMethods = builder.Configuration.GetSection("Cors:AllowedMethods").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithMethods(allowedMethods)
        .WithHeaders(allowedHeaders)
        .WithOrigins(allowedOrigins);
    });
});

var app = builder.Build();

app.UseCors("CorsPolicy");
app.UseMiddleware<BussinessExceptionMiddleware>();

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