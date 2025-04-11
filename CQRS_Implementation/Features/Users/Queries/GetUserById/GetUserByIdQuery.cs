using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Queries.GetUserById;

public class GetUserByIdQuery : IQuery<UserDto>
{
    public Guid Id { get; set; }
}