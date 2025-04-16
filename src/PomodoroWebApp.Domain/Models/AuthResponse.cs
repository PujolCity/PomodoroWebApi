namespace PomodoroWebApp.Domain.Models;

public class AuthResponse
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime TokenExpiry { get; set; }
}
