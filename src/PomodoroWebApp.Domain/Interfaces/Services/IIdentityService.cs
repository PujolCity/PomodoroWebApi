using Microsoft.AspNetCore.Identity;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Models;
using PomodoroWebApp.Domain.Results;

namespace PomodoroWebApp.Domain.Interfaces.Services;

/// <summary>
/// Define las operaciones relacionadas con la autenticación y gestión de identidad de usuarios.
/// </summary>
public interface IIdentityService
{
    /// <summary>
    /// Registra un nuevo usuario en el sistema.
    /// </summary>
    /// <param name="usuario">Instancia del usuario a registrar.</param>
    /// <param name="password">Contraseña del nuevo usuario.</param>
    /// <returns>Resultado del intento de registro, con detalles del éxito o fallo.</returns>
    Task<Result<IdentityResult>> RegisterUserAsync(Usuario usuario, string password);

    /// <summary>
    /// Autentica a un usuario con sus credenciales.
    /// </summary>
    /// <param name="email">Email del usuario.</param>
    /// <param name="password">Contraseña del usuario.</param>
    /// <returns>Resultado de la autenticación. Contiene el usuario si las credenciales son válidas; de lo contrario, un mensaje de error.</returns>
    Task<Result<Usuario>> AuthenticateAsync(string email, string password);

    /// <summary>
    /// Genera un nuevo token JWT para un usuario autenticado.
    /// </summary>
    /// <param name="usuario">Usuario autenticado.</param>
    /// <returns>Resultado con el token JWT y refresh token si la generación fue exitosa; de lo contrario, un mensaje de error.</returns>
    Task<Result<AuthResponse>> GenerateJwtTokenAsync(Usuario usuario);

    /// <summary>
    /// Cambia la contraseña de un usuario.
    /// </summary>
    /// <param name="usuario">Usuario que desea cambiar su contraseña.</param>
    /// <param name="currentPassword">Contraseña actual.</param>
    /// <param name="newPassword">Nueva contraseña.</param>
    /// <returns>Resultado del intento de cambio de contraseña, incluyendo posibles errores si la operación falla.</returns>
    Task<Result<IdentityResult>> ChangePasswordAsync(Usuario usuario, string currentPassword, string newPassword);

    /// <summary>
    /// Refresca el token JWT utilizando un refresh token válido.
    /// </summary>
    /// <param name="jwtToken">Token JWT expirado.</param>
    /// <param name="refreshToken">Refresh token asociado.</param>
    /// <returns>Resultado con un nuevo JWT y refresh token, o un mensaje de error si falla.</returns>
    Task<Result<AuthResponse>> RefreshTokenAsync(string jwtToken, string refreshToken);
}
