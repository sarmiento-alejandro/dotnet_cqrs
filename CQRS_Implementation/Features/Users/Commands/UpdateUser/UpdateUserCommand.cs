using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Commands.UpdateUser;

public class UpdateUserCommand : ICommand<bool>
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
}