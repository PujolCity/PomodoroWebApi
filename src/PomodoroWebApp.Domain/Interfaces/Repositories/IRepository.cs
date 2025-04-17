using System.Linq.Expressions;

namespace PomodoroWebApp.Domain.Interfaces.Repositories;

/// <summary>
/// Interfaz genérica para el repositorio.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
}
