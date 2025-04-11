using CQRS_Implementation.Domain.ReadModels;
using CQRS_Implementation.Domain.Repositories.CommandInterfaces;
using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.Domain.Services;

namespace CQRS_Implementation.Infrastructure.Services;

public class SynchronizationService : ISynchronizationService
{
    private readonly IUserCommandRepository _userCommandRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        
        public SynchronizationService(
            IUserCommandRepository userCommandRepository,
            IUserQueryRepository userQueryRepository)
        {
            _userCommandRepository = userCommandRepository;
            _userQueryRepository = userQueryRepository;
        }
        
        public async Task SynchronizeUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            // Obtener el usuario desde MariaDB
            var user = await _userCommandRepository.GetByIdAsync(userId, cancellationToken);
            
            if (user == null)
                return;
                
            // Crear/actualizar el modelo de lectura
            var existingReadModel = await _userQueryRepository.GetByIdAsync(userId, cancellationToken);
            
            if (existingReadModel == null)
            {
                // Crear nuevo modelo de lectura
                var newReadModel = new UserReadModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Roles = new List<string>(), // Inicializar con valores por defecto
                    Address = null
                };
                
                await _userQueryRepository.CreateAsync(newReadModel, cancellationToken);
            }
            else
            {
                // Actualizar modelo existente
                existingReadModel.Name = user.Name;
                existingReadModel.Email = user.Email;
                existingReadModel.UpdatedAt = user.UpdatedAt;
                
                await _userQueryRepository.UpdateAsync(existingReadModel, cancellationToken);
            }
        }
        
        public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            await _userQueryRepository.DeleteAsync(userId, cancellationToken);
        }
}