using Microsoft.AspNetCore.Identity;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Repositories;
using PomodoroWebApp.Domain.Interfaces.Services;
using PomodoroWebApp.Domain.Results;
using PomodoroWebApp.Domain.ValidatorMessages;

namespace PomodoroWebApp.Application.Services;

public class UserService : IUserService
{
    private readonly UserManager<Usuario> _userManager;

    public UserService(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task<Result<Usuario>> RegistrarUsuarioAsync(Usuario usuario, string password)
    {
        try
        {
            var result = await _userManager.CreateAsync(usuario, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<Usuario>.Fail($"Error al registrar usuario: {errors}");
            }

            return Result<Usuario>.Ok(usuario);
        }
        catch (Exception ex)
        {
            return Result<Usuario>.Fail($"Exception: {ex.Message}");
        }
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
