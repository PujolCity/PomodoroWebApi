namespace PomodoroWebApp.Domain.Entities;

/// <summary>
/// Clase que representa una sesión de descanso en el sistema Pomodoro.
/// </summary>
public class SesionDescanso : PomodoroSesion
{
    public bool EsDescansoLargo { get; set; }
}
