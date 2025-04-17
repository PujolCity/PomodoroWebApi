using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Domain.Entities;

namespace PomodoroWebApp.Application.Extensions;

/// <summary>
/// Clase estática que contiene métodos de extensión para la entidad Usuario.
/// </summary>
public static class UsuarioExtensions
{
    public static Usuario ToEntity(this RegisterRequestDTO request)
    {
        return new Usuario
        {
            Email = request.Email,
            UserName = request.NombreUsuario,
            Nombre = request.Nombre,
            Apellido = request.Apellido
        };
    }
}
