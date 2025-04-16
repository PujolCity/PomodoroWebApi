using PomodoroWebApp.Application.Dto.Request;
using PomodoroWebApp.Domain.Models;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interfaces.Interactor;

public interface ILoginInteractor
{
    Task<Result<AuthResponse>> Execute(LoginRequestDTO request);
}
