using IPP.Application.Responses.Auth;

namespace IPP.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponse?> AuthenticateAsync(string email, string password);
}
