using System.Security.Claims;

namespace PomodoroWebApp.Domain.Interfaces.Services;

/// <summary>
/// Interfaz para el servicio de gestión de tokens de autorización.
/// </summary>
public interface IAuthorizationTokenService
{
    Dictionary<string, string> GetAllClaims();
    ClaimsPrincipal GetUserClaimsPrincipal();
    string? GetCurrentUserId();
}
