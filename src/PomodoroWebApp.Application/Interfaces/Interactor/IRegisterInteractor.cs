using PomodoroWebApp.Application.Dto;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interfaces.Interactor;

public interface IRegisterInteractor
{
    Task<Result<int>> Execute(RegisterRequestDTO request);
}