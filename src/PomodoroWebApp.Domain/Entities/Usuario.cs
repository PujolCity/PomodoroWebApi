namespace PomodoroWebApp.Domain.Entities;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public string NombreUsuario { get; set; }
    public string Password { get; set; }
    public bool Baja { get; set; }

    // Relaciones
    public ICollection<Proyecto> Proyectos { get; set; }
    public ICollection<EstadisticasUsuario> EstadisticasUsuario { get; set; }
    public ICollection<Sesion> Sesiones { get; set; }
}