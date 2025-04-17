namespace PomodoroWebApp.Domain.Models;

/// <summary>
/// Modelo de respuesta de autenticación.
/// </summary>
public class AuthResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime TokenExpiry { get; set; }
}
