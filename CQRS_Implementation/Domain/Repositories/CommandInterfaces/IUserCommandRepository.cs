namespace CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Entities;

public interface IUserCommandRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(User? user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User? user, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);
}