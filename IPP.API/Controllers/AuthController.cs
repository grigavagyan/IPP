using IPP.Application.Auth.Commands.Login;
using IPP.Application.Interfaces;
using IPP.Application.Responses.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly ICommandDispatcher _commandDispatcher;

    public AuthController(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        if (string.IsNullOrWhiteSpace(command.UserName) || string.IsNullOrWhiteSpace(command.Password))
            return BadRequest("Email and password are required.");

        var authResult = await _commandDispatcher.Dispatch<LoginCommand, LoginResponse>(command);
        if (authResult == null) return Unauthorized("Invalid credentials.");

        return Ok(authResult);
    }
}