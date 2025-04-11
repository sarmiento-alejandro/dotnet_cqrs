using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Queries.FindUsersByName;

public class FindUsersByNameQuery : IQuery<IEnumerable<UserDto>>
{
    public string Name { get; set; }
}