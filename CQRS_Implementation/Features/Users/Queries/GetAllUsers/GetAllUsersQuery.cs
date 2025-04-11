using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQuery : IQuery<IEnumerable<UserDto>>
{
    public int? Limit { get; set; }
    public int? Offset { get; set; }
}