using CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Services;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Commands.UpdateUser;

public class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, bool>
{
    private readonly IUserCommandRepository _userRepository;
    private readonly ISynchronizationService _syncService;
        
    public UpdateUserCommandHandler(
        IUserCommandRepository userRepository,
        ISynchronizationService syncService)
    {
        _userRepository = userRepository;
        _syncService = syncService;
    }
        
    public async Task<bool> HandleAsync(UpdateUserCommand command, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(command.Id, cancellationToken);
            
        if (user == null)
        {
            return false;
        }
            
        // Verificar si el email está siendo usado por otro usuario
        if (user.Email != command.Email && await _userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
        {
            throw new ApplicationException("El email ya está siendo utilizado por otro usuario.");
        }
            
        // Actualizar propiedades
        user.Name = command.Name;
        user.Email = command.Email;
        user.UpdatedAt = DateTime.UtcNow;
            
        // Guardar cambios
        await _userRepository.UpdateAsync(user, cancellationToken);
            
        // Sincronizar con MongoDB
        await _syncService.SynchronizeUserAsync(user.Id, cancellationToken);
            
        return true;
    }
}