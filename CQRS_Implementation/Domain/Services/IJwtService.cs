using CQRS_Implementation.Domain.Entities;

namespace CQRS_Implementation.Domain.Services;

public interface IJwtService
{
    Task<string> GenerateTokenAsync(User user);
    Task<string> GenerateRefreshTokenAsync();
    Task<(bool isValid, string userId)> ValidateTokenAsync(string token);
}