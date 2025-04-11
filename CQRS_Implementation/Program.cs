using System.Text;
using CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.Domain.Services;
using CQRS_Implementation.Infrastructure.Data.MariaDb;
using CQRS_Implementation.Infrastructure.Data.MongoDB;
using CQRS_Implementation.Infrastructure.Data.MongoDB.Configurations;
using CQRS_Implementation.Infrastructure.Repositories.Commands;
using CQRS_Implementation.Infrastructure.Repositories.Queries;
using CQRS_Implementation.Infrastructure.Services;
using CQRS_Lib;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


var assemblies = new [] {typeof(Program).Assembly};
builder.Services.AddCqrs(assemblies);

builder.Services.AddDbContext<MariaDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MariaDbConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("MariaDbConnection")),
        mysqlOptions => mysqlOptions.EnableRetryOnFailure()
    )
);

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddScoped<ISynchronizationService, SynchronizationService>();

builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();

builder.Services.AddScoped<IUserCommandRepository, UserCommandRepository>();
builder.Services.AddScoped<IAuthQueryRepository, AuthQueryRepository>();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var key = Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!);

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

// Registrar servicio JWT
builder.Services.AddScoped<IJwtService, JwtService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    
    ApplyMigrations(app);
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();




void ApplyMigrations(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<MariaDbContext>();
            
            // Verifica si hay migraciones pendientes
            if (context.Database.GetPendingMigrations().Any())
            {
                Console.WriteLine("Aplicando migraciones pendientes...");
                context.Database.Migrate();
                Console.WriteLine("Migraciones aplicadas con éxito.");
            }
            else
            {
                Console.WriteLine("No hay migraciones pendientes.");
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ocurrió un error al aplicar las migraciones.");
            
            // En desarrollo, podrías considerar lanzar la excepción
            if (app.Environment.IsDevelopment())
            {
                throw;
            }
        }
    }
}