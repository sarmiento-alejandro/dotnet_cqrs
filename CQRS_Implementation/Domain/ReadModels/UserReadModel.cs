using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CQRS_Implementation.Domain.ReadModels;

public class UserReadModel
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Campos adicionales para consultas optimizadas
    public List<string> Roles { get; set; } = new List<string>();
    public AddressReadModel Address { get; set; }
}

public class AddressReadModel
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
}