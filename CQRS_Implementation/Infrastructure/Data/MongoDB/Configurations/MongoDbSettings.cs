namespace CQRS_Implementation.Infrastructure.Data.MongoDB.Configurations;

public class MongoDbSettings
{
    public string DatabaseName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public CollectionNames CollectionNames { get; set; }
}

public abstract class CollectionNames
{
    public string Users { get; set; }
}