namespace PomodoroWebApp.Application.Dto.Auth;

public class RegisterRequestDTO
{
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string NombreUsuario { get; set; }
    public required string Nombre { get; set; }
    public required string Apellido { get; set; }
}
