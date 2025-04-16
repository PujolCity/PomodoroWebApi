using PomodoroWebApp.Application.Dto.Request;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interfaces.Interactor;

public interface IRegisterInteractor
{
    Task<Result<bool>> Execute(RegisterRequestDTO request);
}