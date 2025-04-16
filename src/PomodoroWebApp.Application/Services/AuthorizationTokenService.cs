using Microsoft.AspNetCore.Http;
using PomodoroWebApp.Domain.Interfaces.Services;
using System.Security.Claims;

namespace PomodoroWebApp.Application.Services;

public class AuthorizationTokenService : IAuthorizationTokenService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public AuthorizationTokenService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Dictionary<string, string> GetAllClaims()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
        return claimsPrincipal == null
            ? throw new InvalidOperationException("No hay usuario autenticado")
            : claimsPrincipal.Claims
            .GroupBy(c => c.Type)
            .ToDictionary(
                g => g.Key,
                g => g.Count() > 1 ? string.Join(", ", g.Select(c => c.Value)) : g.First().Value
            );
    }

    public string? GetCurrentUserId()
    {
        return _httpContextAccessor.HttpContext?
                   .User?
                   .FindFirst(ClaimTypes.NameIdentifier)?
                   .Value;
    }

    public ClaimsPrincipal GetUserClaimsPrincipal()
    {
        return _httpContextAccessor.HttpContext?.User
                      ?? throw new InvalidOperationException("Contexto HTTP no disponible");
    }
}
