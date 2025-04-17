namespace PomodoroWebApp.Application.Dto.Auth;

/// <summary>
/// DTO para la solicitud de inicio de sesión.
/// </summary>
public class LoginRequestDTO
{
    public string Email { get; set; }
    public string Password { get; set; }
}
