using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Users.Commands.DeleteUser;

public class DeleteUserCommand : ICommand<bool>
{
    public Guid Id { get; set; }
}