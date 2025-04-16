using System.Security.Claims;

namespace PomodoroWebApp.Domain.Interfaces.Services;
public interface IAuthorizationTokenService
{
    Dictionary<string, string> GetAllClaims();
    ClaimsPrincipal GetUserClaimsPrincipal();
    string? GetCurrentUserId();
}
