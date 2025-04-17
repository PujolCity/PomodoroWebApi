using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Application.Interfaces.Interactor;

/// <summary>
/// Interfaz para el interactor de registro
/// </summary>
public interface IRegisterInteractor
{
    Task<Result<bool>> Execute(RegisterRequestDTO request);
}