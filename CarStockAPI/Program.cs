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
using CarStockBLL.Map;
using CarStockAPI.Filters;
using CarStockAPI.Extensions;
using CarStockDAL.Data.Repositories.WS;
using CarStockBLL.Hubs;
using CarStockBLL.Services.SignalR_Services;
using CarStockDAL.Data.Interfaces.MaintenanceRepo;
using MediatrBL.Services;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using CarStockAPI.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

//                                              Настройки serilog
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

//                                              Настройка в сваггере для авторизации
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

builder.Services.AddSignalR();

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
builder.Services.AddScoped<IMaintenanceRepository, PostgreMaintenanceRepository>();

builder.Services.AddScoped<ICarService, CarServiceWithMediatr>();
builder.Services.AddScoped<IBrandService, BrandServiceWithMediatr>();
builder.Services.AddScoped<ICarModelService, CarModelServiceWithMediatr>();
builder.Services.AddScoped<IColorService, ColorServiceWithMediatr>();


builder.Services.AddScoped<IUserService, UserServiceWithMediatr>();
builder.Services.AddScoped<IAuthorizeUserService, AuthServiceWithMediatr>();

builder.Services.AddScoped<IMaitenanceService, MaintenanceService>();

builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<CarMapper>();


//builder.Services.AddSingleton<WebSocketHandler>();
//builder.Services.AddSingleton<IHostedService, WsMaintenanceStatusChecker>(); // Фоновая задача 
builder.Services.AddSingleton<IHostedService, SrMaintenanceStatusChecker>(); // для signalR



//                                               Настройка фильтра
builder.Services.AddScoped<RequireAcceptHeaderFilter>();
var jwtConfig = builder.Configuration.GetSection("JwtConfig").Get<JwtConfig>();
builder.Services.AddScoped<ITokenService, TokenService>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<ITokenService>>();
    return new TokenService(jwtConfig.Secret, jwtConfig.Issuer, jwtConfig.Audience, logger);
});



//                                               Используем Autofac для MediatR вместо стандартного DI
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new MediatorModule());
});



//                                              Google
builder.Services.Configure<GoogleConfig>(builder.Configuration.GetSection("Authentication:Google"));




//                                              Аутентификация с JWT
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

//                                              Настройки политик
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
    
    options.AddPolicy("AccountPolicy", policy =>
            policy.RequireClaim("Permission", "CanEditAccount"));
});



//                                              Настройки CORS
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
var allowedHeaders = builder.Configuration.GetSection("Cors:AllowedHeaders").Get<string[]>();
var allowedMethods = builder.Configuration.GetSection("Cors:AllowedMethods").Get<string[]>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader()
        .AllowAnyMethod()
        //policy.WithMethods(allowedMethods)
        //.WithHeaders(allowedHeaders)
        .WithOrigins(allowedOrigins)
        .AllowCredentials();

    });
});




builder.Services.Configure<AllowedPathsOptions>(builder.Configuration.GetSection("AllowedPaths")); // Для использования в middleware проверки
var app = builder.Build();
app.UseCors("CorsPolicy");




//                                               Включаем поддержку WebSocket
var webSocketOptions = new WebSocketOptions
{
    KeepAliveInterval = TimeSpan.FromMinutes(2) // Поддержка соединения
};
app.UseWebSockets(webSocketOptions);

//                                              Установка middleware исключений
app.UseMiddleware<BussinessExceptionMiddleware>();



//                                              Установка middleware проверки тех. работ
app.UseMiddleware<MaintenanceMiddleware>();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.MapControllers();
app.MapHub<NotifierHub>("/notifier"); // NotifierHub обработает запросы по этому пути

app.Run();