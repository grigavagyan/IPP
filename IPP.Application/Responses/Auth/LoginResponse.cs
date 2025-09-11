namespace IPP.Application.Responses.Auth;

public class LoginResponse
{
    public string AccessToken { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
}