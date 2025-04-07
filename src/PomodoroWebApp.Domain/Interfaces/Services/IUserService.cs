using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Domain.Interfaces.Services;

public interface IUserService
{
    Task<Result<Usuario>> RegistrarUsuarioAsync(Usuario usuario, string password);
    Task<Result<bool>> EmailExistsAsync(string email);
    Task<Result<bool>> UserNameExistAsync(string userName);
    Task<Result<Usuario>> GetUserByEmailAsync(string email);
}
