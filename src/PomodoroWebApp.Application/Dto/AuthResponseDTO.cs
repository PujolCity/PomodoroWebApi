namespace PomodoroWebApp.Application.Dto;

public class AuthResponseDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime Expiry { get; set; }
}
