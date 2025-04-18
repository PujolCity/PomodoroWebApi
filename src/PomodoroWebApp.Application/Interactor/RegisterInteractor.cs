using FluentValidation;
using Microsoft.Extensions.Logging;
using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Application.Extensions;
using PomodoroWebApp.Application.Interfaces.Interactor;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interactor;

/// <summary>
/// Clase que implementa la lógica de negocio para el registro de usuarios.
/// </summary>
public class RegisterInteractor : IRegisterInteractor
{
    private readonly ILogger<RegisterInteractor> _logger;
    private readonly IIdentityService _identityService;
    private readonly IValidator<RegisterRequestDTO> _validator;

    public RegisterInteractor(IIdentityService usuarioService,
        IValidator<RegisterRequestDTO> validator,
        ILogger<RegisterInteractor> logger)
    {
        _identityService = usuarioService;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<bool>> Execute(RegisterRequestDTO request)
    {
        _logger.LogInformation("Ejecutando el interactor de registro de usuario.");
        
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<bool>.Fail(validationResult.Errors.ToErrorMessages());
        
        var usuario = request.ToEntity();

        var registroResult = await _identityService.RegisterUserAsync(usuario, request.Password);
        if (registroResult.IsFailed)
            return Result<bool>.Fail(registroResult.Errors);

        return Result<bool>.Ok(registroResult.Value.Succeeded);
    }
}
