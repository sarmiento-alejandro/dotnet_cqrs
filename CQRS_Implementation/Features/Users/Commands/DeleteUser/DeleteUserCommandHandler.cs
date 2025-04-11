using CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Services;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand, bool>
{
    private readonly IUserCommandRepository _userRepository;
    private readonly ISynchronizationService _syncService;
        
    public DeleteUserCommandHandler(
        IUserCommandRepository userRepository,
        ISynchronizationService syncService)
    {
        _userRepository = userRepository;
        _syncService = syncService;
    }
        
    public async Task<bool> HandleAsync(DeleteUserCommand command, CancellationToken cancellationToken = default)
    {
        var exists = await _userRepository.GetByIdAsync(command.Id, cancellationToken) != null;
            
        if (!exists)
        {
            return false;
        }
            
        // Eliminar de MariaDB
        await _userRepository.DeleteAsync(command.Id, cancellationToken);
            
        // Eliminar de MongoDB
        await _syncService.DeleteUserAsync(command.Id, cancellationToken);
            
        return true;
    }
}