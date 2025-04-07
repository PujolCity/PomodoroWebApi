using PomodoroWebApp.Domain.Entities;

namespace PomodoroWebApp.Domain.Interfaces.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUsuarioRepository Usuarios { get; }
    IRepository<Proyecto> Proyectos { get; }
    // Agregar otros repositorios según tus entidades...
    Task<int> CommitAsync();
}
