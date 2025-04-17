using PomodoroWebApp.Domain.Entities;

namespace PomodoroWebApp.Domain.Interfaces.Repositories;

/// <summary>
/// Interfaz para el patrón Unit of Work.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    IRepository<Proyecto> Proyectos { get; }
    Task<int> CommitAsync();
}
