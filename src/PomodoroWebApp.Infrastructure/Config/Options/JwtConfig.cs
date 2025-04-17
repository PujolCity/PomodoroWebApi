namespace PomodoroWebApp.Infrastructure.Config.Options;

/// <summary>
/// Configuración para el manejo de JWT.
/// </summary>
public class JwtConfig
{
    public string Secret { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
    public int RefreshTokenExpiryInDays { get; set; }
}
