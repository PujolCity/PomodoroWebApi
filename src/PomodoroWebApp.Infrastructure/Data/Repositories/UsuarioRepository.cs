using Microsoft.EntityFrameworkCore;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Interfaces.Repositories;

namespace PomodoroWebApp.Infrastructure.Data.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(PomodoroDbContext context) : base(context) { }

    public async Task<bool> EmailExistsAsync(string email)
        => await _context.Usuarios.AnyAsync(u => u.Email == email);

    public async Task<Usuario?> GetByEmailAsync(string email)
        => await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);

    public async Task<bool> UserNameExistAsync(string userName)
    => await _context.Usuarios.AnyAsync(u => u.UserName == userName);
}