using CQRS_Implementation.Domain.ReadModels;

namespace CQRS_Implementation.Domain.Repositories.Queries;

public interface IUserQueryRepository
    {
        Task<UserReadModel> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReadModel>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<UserReadModel>> FindByNameAsync(string name, CancellationToken cancellationToken = default);
        Task<UserReadModel> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task CreateAsync(UserReadModel user, CancellationToken cancellationToken = default);
        Task UpdateAsync(UserReadModel user, CancellationToken cancellationToken = default);
        Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }