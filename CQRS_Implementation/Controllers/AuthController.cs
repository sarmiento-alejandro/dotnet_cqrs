using CQRS_Implementation.DTOs;
using CQRS_Implementation.Features.Auth.Commands.Login;
using CQRS_Lib.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Implementation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;
        
    public AuthController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }
        
    [HttpPost("login")]
    public async Task<ActionResult<LoginResult>> Login([FromBody] LoginCommand command)
    {
        var result = await _commandDispatcher.DispatchAsync<LoginCommand, LoginResult>(command);
            
        if (!result.Success)
        {
            return Unauthorized(new { message = result.Error });
        }
            
        return Ok(result);
    }
}