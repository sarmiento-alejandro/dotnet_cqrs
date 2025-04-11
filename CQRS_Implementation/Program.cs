using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.Infrastructure.Data.MariaDb;
using CQRS_Implementation.Infrastructure.Data.MongoDB;
using CQRS_Implementation.Infrastructure.Data.MongoDB.Configurations;
using CQRS_Implementation.Infrastructure.Repositories.Queries;
using CQRS_Lib;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
builder.Services.AddScoped<IUserQueryRepository, UserQueryRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();