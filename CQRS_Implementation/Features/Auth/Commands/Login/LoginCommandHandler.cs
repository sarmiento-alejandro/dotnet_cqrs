using CQRS_Implementation.Domain.Repositories.Queries;
using CQRS_Implementation.Domain.Services;
using CQRS_Implementation.DTOs;
using CQRS_Lib.BaseInterfaces;
using CQRS_Utilities.PasswordHasherUtility;

namespace CQRS_Implementation.Features.Auth.Commands.Login;

public class LoginCommandHandler : ICommandHandler<LoginCommand, LoginResult>
    {
        private readonly IAuthQueryRepository _authQueryRepository;
        private readonly IJwtService _jwtService;
        
        public LoginCommandHandler(
            IAuthQueryRepository authQueryRepository,
            IJwtService jwtService)
        {
            _authQueryRepository = authQueryRepository;
            _jwtService = jwtService;
        }
        
        public async Task<LoginResult> HandleAsync(LoginCommand command, CancellationToken cancellationToken = default)
        {
            // Buscar usuario por email en MongoDB (más rápido para consultas)
            var user = await _authQueryRepository.FindByEmailAsync(command.Email, cancellationToken);
            
            if (user == null)
            {
                return new LoginResult
                {
                    Success = false,
                    Error = "Usuario o contraseña incorrectos"
                };
            }
            
             bool isPasswordValid = PasswordHasher.VerifyPassword(command.Password, user.PasswordHash);
            
            if (!isPasswordValid)
            {
                return new LoginResult
                {
                    Success = false,
                    Error = "Usuario o contraseña incorrectos"
                };
            }
            
            // Generar tokens
            string token = await _jwtService.GenerateTokenAsync(new Domain.Entities.User 
            { 
                Id = user.Id, 
                Email = user.Email, 
                Name = user.Name 
            });
            
            string refreshToken = await _jwtService.GenerateRefreshTokenAsync();
            
            // En un caso real, almacenarías el refreshToken en la base de datos
            // Aquí omitimos ese paso por simplicidad
            
            return new LoginResult
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken
            };
        }
    }