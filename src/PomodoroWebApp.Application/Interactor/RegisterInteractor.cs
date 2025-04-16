using FluentValidation;
using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Application.Extensions;
using PomodoroWebApp.Application.Interfaces.Interactor;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interactor;

public class RegisterInteractor : IRegisterInteractor
{
    private readonly IIdentityService _identityService;
    private readonly IValidator<RegisterRequestDTO> _validator;
    public RegisterInteractor(IIdentityService usuarioService,
        IValidator<RegisterRequestDTO> validator)
    {
        _identityService = usuarioService;
        _validator = validator;
    }

    public async Task<Result<bool>> Execute(RegisterRequestDTO request)
    {
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
