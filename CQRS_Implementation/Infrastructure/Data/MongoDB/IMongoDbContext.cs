using CQRS_Implementation.Infrastructure.Data.MongoDB.Configurations;
using MongoDB.Driver;

namespace CQRS_Implementation.Infrastructure.Data.MongoDB;

public interface IMongoDbContext
{
    IMongoCollection<T> GetCollection<T>(string name);
    MongoDbSettings Settings { get; }
}