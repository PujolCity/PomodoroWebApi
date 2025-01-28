namespace PomodoroWebApp.Domain.Entities;

public class Proyecto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public int UsuarioId { get; set; }

    // Relaciones
    public Usuario Usuario { get; set; }
    public List<Tarea> Tareas { get; set; } = new();
}