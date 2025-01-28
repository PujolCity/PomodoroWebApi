namespace PomodoroWebApp.Domain.Entities;

public class EstadisticasUsuario
{
    public int EstadisticasUsuarioId { get; set; }
    public int UsuarioId { get; set; }
    public DateTime Fecha { get; set; }  // La fecha de las estadísticas (diaria, semanal, etc.)

    // Estadísticas generales
    public int ProyectosCompletados { get; set; }
    public int TareasCompletadas { get; set; }
    public int CiclosCompletados { get; set; }

    // Estadísticas por tipo de Pomodoro (Trabajo vs. Descanso)
    public int MinutosTrabajo { get; set; }
    public int MinutosDescanso { get; set; }

    // Estadísticas de ciclo (para análisis por cada ciclo)
    public int MaximoTrabajoSinDescanso { get; set; }  // El máximo tiempo de trabajo sin descanso

    // Estadísticas relacionadas con proyectos o tareas específicas
    public int PromedioDescanso { get; set; }  // Promedio de minutos de descanso por ciclo
    public int MinutosTrabajoPorTarea { get; set; }  // Minutos de trabajo por tarea
    public int MinutosTrabajoPorProyecto { get; set; }  // Minutos de trabajo por proyecto

    public Usuario Usuario { get; set; }

}