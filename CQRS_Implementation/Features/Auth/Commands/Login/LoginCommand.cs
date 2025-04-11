using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;

namespace CQRS_Implementation.Features.Auth.Commands.Login;

public class LoginCommand : ICommand<LoginResult>
{
    public string Email { get; set; }
    public string Password { get; set; }
}