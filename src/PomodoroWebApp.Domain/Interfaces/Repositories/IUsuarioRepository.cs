using PomodoroWebApp.Domain.Entities;

namespace PomodoroWebApp.Domain.Interfaces.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task<bool> UserNameExistAsync(string userName);
}