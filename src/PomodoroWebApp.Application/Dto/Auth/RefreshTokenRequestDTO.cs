namespace PomodoroWebApp.Application.Dto.Auth;

/// <summary>
/// DTO para la solicitud de un nuevo token de acceso utilizando un token de actualización.
/// </summary>
public class RefreshTokenRequestDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
