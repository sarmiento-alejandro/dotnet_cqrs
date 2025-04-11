namespace CQRS_Implementation.DTOs;

public class LoginResult
{
    public bool Success { get; set; }
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public string Error { get; set; }
}