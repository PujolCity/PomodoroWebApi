namespace PomodoroWebApp.Domain.Entities;

/// <summary>
/// Representa una sesión de trabajo o descanso.
/// </summary>
public class Sesion
{
    public int Id { get; set; }
    public int Ciclos { get; set; }
    public SesionTrabajo SesionTrabajo { get; set; }
    public SesionDescanso DescansoCorto { get; set; }
    public SesionDescanso DescansoLargo { get; set; }

    // Relaciones
    public int? TareaId { get; set; }
    public Tarea Tarea { get; set; }
    public int UsuarioId { get; set; }
    public Usuario Usuario { get; set; }
}