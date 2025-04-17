using Microsoft.AspNetCore.Identity;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Results;
using PomodoroWebApp.Domain.ValidatorMessages;

namespace PomodoroWebApp.Application.Services;

/// <summary>
/// Implementación del servicio de usuario.
/// </summary>
public class UserService : IUserService
{
    private readonly UserManager<Usuario> _userManager;

    public UserService(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<bool>> EmailExistsAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user != null
            ? Result<bool>.Fail(AppValidatorMessage.EmailExistError())
            : Result<bool>.Ok(false);
    }

    public async Task<Result<bool>> UserNameExistAsync(string userName)
    {
        var user = await _userManager.FindByNameAsync(userName);
        return user != null
            ? Result<bool>.Fail(AppValidatorMessage.UserNameExistError())
            : Result<bool>.Ok(false);
    }

    public async Task<Result<Usuario>> GetUserByEmailAsync(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        return user == null
            ? Result<Usuario>.Fail("Usuario no encontrado")
            : Result<Usuario>.Ok(user);
    }
}
