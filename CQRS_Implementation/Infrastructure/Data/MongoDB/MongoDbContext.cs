using CQRS_Implementation.Infrastructure.Data.MongoDB.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CQRS_Implementation.Infrastructure.Data.MongoDB;

public class MongoDbContext : IMongoDbContext
{
    private readonly IMongoDatabase _database;
    public MongoDbSettings Settings { get; }
        
    public MongoDbContext(IOptions<MongoDbSettings> settings, IConfiguration configuration)
    {
        Settings = settings.Value;
        
        var connectionString = configuration.GetConnectionString("MongoDbConnection");
        
        if (connectionString != null && !connectionString.Contains("username") && !connectionString.Contains("password"))
        {
            var mongoUrl = new MongoUrlBuilder(connectionString)
            {
                Username = Settings.Username,
                Password = Settings.Password
            }.ToMongoUrl();
            
            var client = new MongoClient(mongoUrl);
            _database = client.GetDatabase(Settings.DatabaseName);
        }
        else
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(Settings.DatabaseName);
        }
    }
        
    public IMongoCollection<T> GetCollection<T>(string name)
    {
        return _database.GetCollection<T>(name);
    }
}