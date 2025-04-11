using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Commands.CreateUser;

public class CreateUserCommand : ICommand<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}