using Microsoft.AspNetCore.Identity;

namespace PomodoroWebApp.Domain.Entities;

/// <summary>
/// Clase que representa un usuario del sistema.
/// </summary>
public class Usuario : IdentityUser<int>
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public bool Baja { get; set; }

    // Relaciones
    public ICollection<Proyecto> Proyectos { get; set; }
    public ICollection<EstadisticasUsuario> EstadisticasUsuario { get; set; }
    public ICollection<Sesion> Sesiones { get; set; }
}