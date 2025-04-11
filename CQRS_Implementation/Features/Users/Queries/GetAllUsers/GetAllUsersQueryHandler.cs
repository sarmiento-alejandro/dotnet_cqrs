using CQRS_Implementation.Domain.ReadModels;
using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserQueryRepository _userRepository;
        
    public GetAllUsersQueryHandler(IUserQueryRepository userRepository)
    {
        _userRepository = userRepository;
    }
        
    public async Task<IEnumerable<UserDto>> HandleAsync(GetAllUsersQuery query, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.GetAllAsync(cancellationToken);
            
        // Aplicar paginación si se especifica
        if (query.Offset.HasValue)
        {
            users = users.Skip(query.Offset.Value);
        }
            
        if (query.Limit.HasValue)
        {
            users = users.Take(query.Limit.Value);
        }
            
        return users.Select(UserReadModel.MapToDto);
    }
    
}