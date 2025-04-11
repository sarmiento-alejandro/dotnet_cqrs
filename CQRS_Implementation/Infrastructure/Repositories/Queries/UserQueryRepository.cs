using CQRS_Implementation.Domain.ReadModels;
using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.Infrastructure.Data.MongoDB;
using MongoDB.Driver;

namespace CQRS_Implementation.Infrastructure.Repositories.Queries;

public class UserQueryRepository(IMongoDbContext context) : IUserQueryRepository
{
        private readonly IMongoCollection<UserReadModel> _collection = context.GetCollection<UserReadModel>(context.Settings.CollectionNames.Users);

        public async Task<UserReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(u => u.Id == id).FirstOrDefaultAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<UserReadModel>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _collection.Find(_ => true).ToListAsync(cancellationToken);
        }
        
        public async Task<IEnumerable<UserReadModel>> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            var filter = Builders<UserReadModel>.Filter.Regex(u => u.Name, new MongoDB.Bson.BsonRegularExpression(name, "i"));
            return await _collection.Find(filter).ToListAsync(cancellationToken);
        }
        
        public async Task<UserReadModel> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _collection.Find(u => u.Email == email).FirstOrDefaultAsync(cancellationToken);
        }
        
        public async Task CreateAsync(UserReadModel user, CancellationToken cancellationToken = default)
        {
            await _collection.InsertOneAsync(user, null, cancellationToken);
        }
        
        public async Task UpdateAsync(UserReadModel user, CancellationToken cancellationToken = default)
        {
            await _collection.ReplaceOneAsync(u => u.Id == user.Id, user, new ReplaceOptions { IsUpsert = false }, cancellationToken);
        }
        
        public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            await _collection.DeleteOneAsync(u => u.Id == id, cancellationToken);
        }
    }