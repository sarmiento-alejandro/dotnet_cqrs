using CQRS_Implementation.Domain.ReadModels;
using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Queries.FindUsersByName;

public class FindUsersByNameQueryHandler : IQueryHandler<FindUsersByNameQuery, IEnumerable<UserDto>>
{
    private readonly IUserQueryRepository _userRepository;
        
    public FindUsersByNameQueryHandler(IUserQueryRepository userRepository)
    {
        _userRepository = userRepository;
    }
        
    public async Task<IEnumerable<UserDto>> HandleAsync(FindUsersByNameQuery query, CancellationToken cancellationToken = default)
    {
        var users = await _userRepository.FindByNameAsync(query.Name, cancellationToken);
        return users.Select(UserReadModel.MapToDto);
    }
        
    
}