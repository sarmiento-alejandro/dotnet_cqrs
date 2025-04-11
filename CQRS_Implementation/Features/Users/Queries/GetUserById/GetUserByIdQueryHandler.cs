using CQRS_Implementation.Domain.ReadModels;
using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserQueryRepository _userRepository;
        
    public GetUserByIdQueryHandler(IUserQueryRepository userRepository)
    {
        _userRepository = userRepository;
    }
        
    public async Task<UserDto> HandleAsync(GetUserByIdQuery query, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByIdAsync(query.Id, cancellationToken);
            
        if (user == null)
            return null;
                
        return UserReadModel.MapToDto(user);
    }
    
}