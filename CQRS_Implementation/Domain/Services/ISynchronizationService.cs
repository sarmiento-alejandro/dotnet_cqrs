namespace CQRS_Implementation.Domain.Services;

public interface ISynchronizationService
{
    Task SynchronizeUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default);
}