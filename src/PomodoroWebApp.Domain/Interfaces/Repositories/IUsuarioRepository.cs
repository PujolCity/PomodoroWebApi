using PomodoroWebApp.Domain.Entities;

namespace PomodoroWebApp.Domain.Interfaces.Repositories;

/// <summary>
/// Interfaz para el repositorio de usuarios.
/// </summary>
public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UserNameExistAsync(string userName);
}