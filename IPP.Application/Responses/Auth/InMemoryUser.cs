namespace IPP.Application.Responses.Auth;

public class InMemoryUser
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Role { get; set; } = default!;
    public string Name { get; set; } = default!;
}
