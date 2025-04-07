using FluentValidation;
using PomodoroWebApp.Application.Dto;
using PomodoroWebApp.Application.Extensions;
using PomodoroWebApp.Application.Interfaces.Interactor;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interactor;

public class RegisterInteractor : IRegisterInteractor
{
    private readonly IUserService _usuarioService;
    private readonly IValidator<RegisterRequestDTO> _validator;
    public RegisterInteractor(IUserService usuarioService,
        IValidator<RegisterRequestDTO> validator)
    {
        _usuarioService = usuarioService;
        _validator = validator;
    }

    public async Task<Result<int>> Execute(RegisterRequestDTO request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Result<int>.Fail(validationResult.Errors.ToErrorMessages());

        var usuario = request.ToEntity();

        var registroResult = await _usuarioService.RegistrarUsuarioAsync(usuario, request.Password);
        if (registroResult.IsFailed)
            return Result<int>.Fail(registroResult.Errors);

        return Result<int>.Ok(registroResult.Value.Id);
    }
}
