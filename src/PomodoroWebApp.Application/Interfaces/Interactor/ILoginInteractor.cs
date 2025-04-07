using PomodoroWebApp.Application.Dto;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interfaces.Interactor;

public interface ILoginInteractor
{
    Task<Result<AuthResponseDTO>> Execute(LoginRequestDTO request);
}
