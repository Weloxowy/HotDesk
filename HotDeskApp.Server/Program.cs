using System.Reflection;
using System.Text;
using DotNetEnv;
using FluentMigrator.Runner;
using gatherly.server.Persistence.Tokens.BlacklistToken;
using HotDeskApp.Server.Models.UserEntity.Repositories;
using HotDeskApp.Server.Models.UserEntity.Services;
using HotDeskApp.Server.Persistance.UserEntity.Repositories;
using Microsoft.EntityFrameworkCore;
using NHibernate;
using HotDeskApp.Server.Infrastructure;
using HotDeskApp.Server.Models.Desk.Repositories;
using HotDeskApp.Server.Models.Desk.Services;
using HotDeskApp.Server.Models.Location.Repositories;
using HotDeskApp.Server.Models.Location.Services;
using HotDeskApp.Server.Models.Reservation.Repositories;
using HotDeskApp.Server.Models.Reservation.Services;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Repositories;
using HotDeskApp.Server.Models.Tokens.BlacklistToken.Services;
using HotDeskApp.Server.Models.Tokens.JwtToken.Services;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Repositories;
using HotDeskApp.Server.Models.Tokens.RefreshToken.Services;
using HotDeskApp.Server.Persistance.Desk.Repositories;
using HotDeskApp.Server.Persistance.Location.Repositories;
using HotDeskApp.Server.Persistance.Reservation.Repositories;
using HotDeskApp.Server.Persistance.Tokens.BlacklistToken.Repositories;
using HotDeskApp.Server.Persistance.Tokens.RefreshToken.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

Env.Load(".env");

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddSingleton<ISessionFactory>(provider => { return NHibernateHelper.SessionFactory; });

builder.Services.AddScoped(provider =>
{
    var sessionFactory = provider.GetService<ISessionFactory>();
    return sessionFactory.OpenSession();
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserEntityService, UserEntityService>();
builder.Services.AddScoped<IUserEntityRepository, UserEntityRepository>();
builder.Services.AddScoped<IDeskService, DeskService>();
builder.Services.AddScoped<IDeskRepository, DeskRepository>();
builder.Services.AddScoped<IBlacklistTokenService, BlacklistTokenService>();
builder.Services.AddScoped<IBlacklistTokenRepository, BlacklistTokenRepository>();
builder.Services.AddScoped<IRefreshTokenService, RefreshTokenService>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<TokenHelper>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Konfiguracja Swaggera
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Gatherly API", Version = "v1.1" });
    /*
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
     $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
*/

    // Konfiguracja JWT
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
              Enter 'Bearer' [space] and then your token in the text input below.
              \r\n\r\nExample: 'Bearer 12345abcdef'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };

    c.AddSecurityDefinition("Bearer", securityScheme);

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

//Authentication and Authorization
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = Env.GetString("ISSUER"),
        ValidAudience = Env.GetString("AUDIENCE"),
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Env.GetString("SECRET")))
        //ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            //Console.WriteLine("Authentication failed: " + context.Exception.Message);
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                context.Response.Headers.Add("Token-Expired", "true");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            //Console.WriteLine("Authentication challenge: " + context.Error + " " + context.ErrorDescription);
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            var result = JsonConvert.SerializeObject(new { error = "You are not authorized" });
            return context.Response.WriteAsync(result);
        },
        OnMessageReceived = context =>
        {
            //Console.WriteLine("Token received: " + context.Token);
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            //Console.WriteLine("Token validated for user: " + context.Principal.Identity.Name);
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.WithOrigins(Env.GetString("AUDIENCE"))
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()
            //.WithExposedHeaders("Content-Disposition")
            .SetPreflightMaxAge(TimeSpan.FromMinutes(10));
    });
});

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
        , sqlOptions => sqlOptions.MigrationsAssembly(typeof(DataContext).Assembly.GetName().Name))
);

builder.Services.AddFluentMigratorCore()
    .ConfigureRunner(c =>
    {
        c.AddSqlServer2016()
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
            .ScanIn(Assembly.GetExecutingAssembly()).For.All();
    })
    .AddLogging(config => config.AddFluentMigratorConsole());


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"); });
}

using var scope = app.Services.CreateScope();

var migrator = scope.ServiceProvider.GetService<IMigrationRunner>();

if (migrator != null)
{
    migrator.ListMigrations();
    migrator.MigrateUp();
}
else
{
    throw new Exception("Migration fault");
}

app.UseRouting();

app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["Authorization"];
    if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer ")) context.Request.Headers["Authorization"] = token;
    await next();
});

app.UseCors("AllowSpecificOrigin");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();


public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
}