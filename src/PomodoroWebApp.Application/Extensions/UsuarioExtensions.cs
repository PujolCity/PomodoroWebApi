using PomodoroWebApp.Application.Dto.Auth;
using PomodoroWebApp.Domain.Entities;

namespace PomodoroWebApp.Application.Extensions;

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
