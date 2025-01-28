namespace PomodoroWebApp.Domain.Entities;

public abstract class PomodoroSesion
{
    public int Id { get; set; }
    public DateTime HoraInicio { get; set; }
    public DateTime HoraFin { get; set; }
    public int Duracion => (int)(HoraFin - HoraInicio).TotalMinutes;
}