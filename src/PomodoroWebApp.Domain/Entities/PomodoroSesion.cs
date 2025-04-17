namespace PomodoroWebApp.Domain.Entities;

/// <summary>
/// Clase base para las sesiones de Pomodoro.
/// </summary>
public abstract class PomodoroSesion
{
    public int Id { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraFin { get; set; }
    public int Duracion => (int)(HoraFin - HoraInicio).TotalMinutes;
}