using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Application.Interfaces.Interactor;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Models;
using PomodoroWebApp.Domain.Results;
using PomodoroWebApp.Domain.ValidatorMessages;

namespace PomodoroWebApp.Controllers;

/// <summary>
/// Controlador para autenticación y gestión de usuarios.
/// </summary>
[Route("api/v{version:apiVersion}/users")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IRegisterInteractor _registerInteractor;
    private readonly ILoginInteractor _loginInteractor;
    private readonly IAuthorizationTokenService _authorizationTokenService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IRegisterInteractor registerInteractor,
        ILoginInteractor loginInteractor,
        IAuthorizationTokenService authorizationTokenService,
        ILogger<AuthController> logger)
    {
        _registerInteractor = registerInteractor;
        _loginInteractor = loginInteractor;
        _authorizationTokenService = authorizationTokenService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los claims del usuario autenticado.
    /// ATENCION: Este endpoint solo es para fines de prueba y no debe ser utilizado en producción.
    /// </summary>
    /// <returns></returns>
#if !DEBUG
        [ApiExplorerSettings(IgnoreApi = true)]
#endif
    [HttpGet("me")]
    //[OnlyInEnvironment("DESARROLLO")]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(AppValidatorMessage), 401)]
    public IActionResult GetAllClaims()
    {
        _logger.LogTrace("Este es un mensaje TRACE");
        _logger.LogDebug("Este es un mensaje DEBUG");
        _logger.LogInformation("Este es un mensaje INFO");
        _logger.LogWarning("Este es un mensaje WARNING");
        _logger.LogError("Este es un mensaje ERROR");
        _logger.LogCritical("Este es un mensaje CRITICAL");

        var user = _authorizationTokenService.GetAllClaims();

        if (user == null)
            return Unauthorized(AppValidatorMessage.InvalidTokenError());

        return Ok(user);
    }

    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IErrorMessage), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        var result = await _registerInteractor.Execute(request);
        return result.IsSuccess ? Ok() : BadRequest(result.Errors);
    }

    /// <summary>
    /// Iniciar sesión en el sistema.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IErrorMessage), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        var result = await _loginInteractor.Execute(request);

        if (result.IsFailed)
            return Unauthorized(result.Errors);

        return Ok(result.Value);
    }
}
