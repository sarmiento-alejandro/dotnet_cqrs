using CQRS_Implementation.Domain.Entities;
using CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Services;
using CQRS_Lib.BaseInterfaces;
using Utilities.PasswordHasherUtility;

namespace CQRS_Implementation.Features.Users.Commands.CreateUser;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    private readonly IUserCommandRepository _userRepository;
    private readonly ISynchronizationService _syncService;
        
    public CreateUserCommandHandler(
        IUserCommandRepository userRepository,
        ISynchronizationService syncService)
    {
        _userRepository = userRepository;
        _syncService = syncService;
    }
        
    public async Task<Guid> HandleAsync(CreateUserCommand command, CancellationToken cancellationToken = default)
    {
        // Validación
        if (await _userRepository.ExistsByEmailAsync(command.Email, cancellationToken))
        {
            throw new ApplicationException("Un usuario con este email ya existe.");
        }
            
        // Crear entidad
        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Email = command.Email,
            PasswordHash = PasswordHasher.HashPassword(command.Password), // Requiere el paquete BCrypt.Net-Next
            CreatedAt = DateTime.UtcNow
        };
            
        // Persistir en base de datos
        await _userRepository.AddAsync(user, cancellationToken);
            
        // Sincronizar con MongoDB
        await _syncService.SynchronizeUserAsync(user.Id, cancellationToken);
            
        return user.Id;
    }
}