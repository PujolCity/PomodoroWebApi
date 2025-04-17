namespace PomodoroWebApp.Domain.ValidatorMessages;
using Domain.Results;

/// <summary>
/// Clase que contiene mensajes de error para la validación de la aplicación.
/// </summary>
public static class AppValidatorMessage
{
    public static ErrorMessage UserRegisterError(string message) => new("Usuario-000", $"Error al registrar usuario: {message}");
    public static ErrorMessage UnexpectedError(string message) => new("Usuario-001", $"Error inesperado : {message}");
    public static ErrorMessage EmailExistError() => new("Usuario-002", $"Este email ya existe");
    public static ErrorMessage UserNameExistError() => new("Usuario-003", $"Este nombre de usuario ya existe");
    public static ErrorMessage UserNotFoundError() => new("Usuario-004", $"No se encuentra el usuario");
    public static ErrorMessage InvalidCredentialsError() => new("Usuario-005", $"Credenciales invalidas");
    public static ErrorMessage InvalidTokenError() => new("Usuario-006", $"No se pudo encontrar el ID del usuario en el token.");
}
