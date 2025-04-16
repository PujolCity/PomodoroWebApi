using PomodoroWebApp.Application.Dto.Request;
using PomodoroWebApp.Application.Interfaces.Interactor;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Models;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interactor;

public class LoginInteractor : ILoginInteractor
{
    private readonly IIdentityService _identityService;

    public LoginInteractor(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<Result<AuthResponse>> Execute(LoginRequestDTO request)
    {

        var user = await _identityService.AuthenticateAsync(request.Email, request.Password);
        if (user.IsFailed) 
            return Result<AuthResponse>.Fail(user.Errors);

        var token = await _identityService.GenerateJwtTokenAsync(user.Value);
        return Result<AuthResponse>.Ok(token.Value);
    }
}
