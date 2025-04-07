using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using PomodoroWebApp.Application.Dto;
using PomodoroWebApp.Application.Interfaces.Interactor;

namespace PomodoroWebApp.Controllers;

[Route("api/v{version:apiVersion}/users")]

[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IRegisterInteractor _registerInteractor;

    public AuthController(IRegisterInteractor registerInteractor)
    {
        _registerInteractor = registerInteractor;
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyProfile()
    {
        // Obtiene el ID del usuario desde el token JWT
        var userId = User.FindFirst("id")?.Value;

        if (userId == null)
            return Unauthorized("No se pudo encontrar el ID del usuario en el token.");

        var user = Guid.Parse(userId);
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
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = new
        {
            Success = true,
            Message = "todo ok",
            Data = "TODA LA DATA"
        };

        if (!result.Success)
            return Unauthorized(result.Message);

        return Ok(result.Data);
    }
}
