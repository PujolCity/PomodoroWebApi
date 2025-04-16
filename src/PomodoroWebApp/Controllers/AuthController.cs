using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Application.Interfaces.Interactor;
using PomodoroWebApp.Domain.Interfaces.Services;

namespace PomodoroWebApp.Controllers;

[Route("api/v{version:apiVersion}/users")]

[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IRegisterInteractor _registerInteractor;
    private readonly ILoginInteractor _loginInteractor;
    private readonly IAuthorizationTokenService _authorizationTokenService;

    public AuthController(IRegisterInteractor registerInteractor,
        ILoginInteractor loginInteractor,
        IAuthorizationTokenService authorizationTokenService)
    {
        _registerInteractor = registerInteractor;
        _loginInteractor = loginInteractor;
        _authorizationTokenService = authorizationTokenService;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        // Obtiene el ID del usuario desde el token JWT
        var user = _authorizationTokenService.GetAllClaims();

        if (user == null)
            return Unauthorized("No se pudo encontrar el ID del usuario en el token.");

        return Ok(user);
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        var result = await _registerInteractor.Execute(request);
        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var result = await _loginInteractor.Execute(request);

        if (result.IsFailed)
            return Unauthorized(result.Errors);

        return Ok(result.Value);
    }
}
