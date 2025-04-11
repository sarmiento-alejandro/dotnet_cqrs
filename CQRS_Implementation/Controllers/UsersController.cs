using CQRS_Implementation.DTOs;
using CQRS_Implementation.Features.Users.Commands.CreateUser;
using CQRS_Implementation.Features.Users.Commands.DeleteUser;
using CQRS_Implementation.Features.Users.Commands.UpdateUser;
using CQRS_Implementation.Features.Users.Queries.FindUsersByName;
using CQRS_Implementation.Features.Users.Queries.GetAllUsers;
using CQRS_Implementation.Features.Users.Queries.GetUserById;
using CQRS_Lib.Dispatchers;
using Microsoft.AspNetCore.Mvc;

namespace CQRS_Implementation.Controllers;

[ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        
        public UsersController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll([FromQuery] int? limit, [FromQuery] int? offset)
        {
            var query = new GetAllUsersQuery { Limit = limit, Offset = offset };
            var result = await _queryDispatcher.DispatchAsync<GetAllUsersQuery, IEnumerable<UserDto>>(query);
            return Ok(result);
        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var query = new GetUserByIdQuery { Id = id };
            var result = await _queryDispatcher.DispatchAsync<GetUserByIdQuery, UserDto>(query);
            
            if (result == null)
                return NotFound();
                
            return Ok(result);
        }
        
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<UserDto>>> SearchByName([FromQuery] string name)
        {
            var query = new FindUsersByNameQuery { Name = name };
            var result = await _queryDispatcher.DispatchAsync<FindUsersByNameQuery, IEnumerable<UserDto>>(query);
            return Ok(result);
        }
        
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateUserCommand command)
        {
            try
            {
                var userId = await _commandDispatcher.DispatchAsync<CreateUserCommand, Guid>(command);
                return CreatedAtAction(nameof(GetById), new { id = userId }, userId);
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateUserCommand command)
        {
            if (id != command.Id)
                return BadRequest(new { message = "ID en la URL no coincide con el ID en el cuerpo" });
                
            try
            {
                var success = await _commandDispatcher.DispatchAsync<UpdateUserCommand, bool>(command);
                
                if (!success)
                    return NotFound();
                    
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteUserCommand { Id = id };
            var success = await _commandDispatcher.DispatchAsync<DeleteUserCommand, bool>(command);
            
            if (!success)
                return NotFound();
                
            return NoContent();
        }
    }