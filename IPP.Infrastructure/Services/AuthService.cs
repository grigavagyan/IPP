using IPP.Application.Interfaces;
using IPP.Application.Responses.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly List<InMemoryUser> _users;
    private readonly IConfiguration _config;

    public AuthService(IConfiguration config)
    {
        _config = config;
        _users = new List<InMemoryUser>
        {
            new InMemoryUser { Email = "admin@demo.com", Password = "Pass@123", Role = "Admin", Name = "Admin" },
            new InMemoryUser { Email = "user@demo.com",  Password = "Pass@123", Role = "User",  Name = "User" }
        };
    }

    public Task<AuthResponse?> AuthenticateAsync(string email, string password)
    {
        var user = _users.SingleOrDefault(u =>
            u.Email.Equals(email, StringComparison.OrdinalIgnoreCase) && u.Password == password);

        if (user == null) return Task.FromResult<AuthResponse?>(null);

        var jwtSection = _config.GetSection("Jwt");
        var key = jwtSection["Key"];
        var issuer = jwtSection["Issuer"];
        var audience = jwtSection["Audience"];
        var expiresMinutes = int.Parse(jwtSection["ExpiresMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var keyBytes = Encoding.UTF8.GetBytes(key!);
        var signingKey = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: expiresAt,
            signingCredentials: creds
        );

        var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

        var response = new AuthResponse
        {
            AccessToken = tokenStr,
            ExpiresAt = expiresAt
        };

        return Task.FromResult<AuthResponse?>(response);
    }
}