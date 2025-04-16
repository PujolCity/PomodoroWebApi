using Microsoft.AspNetCore.Identity;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Models;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Domain.Interfaces.Services;

public interface IIdentityService
{
    Task<Result<IdentityResult>> RegisterUserAsync(Usuario usuario, string password);
    Task<Result<Usuario>> AuthenticateAsync(string email, string password);
    Task<Result<AuthResponse>> GenerateJwtTokenAsync(Usuario usuario);
    Task<Result<IdentityResult>> ChangePasswordAsync(Usuario usuario, string currentPassword, string newPassword);
}
