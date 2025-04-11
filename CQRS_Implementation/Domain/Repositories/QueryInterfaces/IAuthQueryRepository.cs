using CQRS_Implementation.Domain.Entities;

namespace CQRS_Implementation.Domain.Repositories.Queries;

public interface IAuthQueryRepository
{
    Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
}