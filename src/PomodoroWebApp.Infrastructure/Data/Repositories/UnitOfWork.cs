using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Repositories;

namespace PomodoroWebApp.Infrastructure.Data.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PomodoroDbContext _context;
    private IUsuarioRepository? _usuarios;
    private IRepository<Proyecto>? _proyectos;

    public UnitOfWork(PomodoroDbContext context)
    {
        _context = context;
    }

    public IUsuarioRepository Usuarios => _usuarios ??= new UsuarioRepository(_context);

    public IRepository<Proyecto> Proyectos => _proyectos ??= new Repository<Proyecto>(_context);

    public async Task<int> CommitAsync() => await _context.SaveChangesAsync();

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}