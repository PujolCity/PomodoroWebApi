using PomodoroWebApp.Domain.Enums;

namespace PomodoroWebApp.Domain.Entities;

public class Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; }
    public Prioridad Prioridad { get; set; }

    // Relaciones
    public int ProyectoId { get; set; }
    public Proyecto Proyecto { get; set; }
    public List<Sesion> Sesiones { get; set; } = [];
}