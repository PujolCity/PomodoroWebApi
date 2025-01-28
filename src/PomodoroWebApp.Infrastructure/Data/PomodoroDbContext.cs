using Microsoft.EntityFrameworkCore;
using PomodoroWebApp.Domain.Entities;
using PomodoroWebApp.Domain.Enums;

namespace PomodoroWebApp.Infrastructure.Data;

public class PomodoroDbContext : DbContext
{
    public PomodoroDbContext(DbContextOptions<PomodoroDbContext> options)
        : base(options) { }

    // Tablas
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Proyecto> Proyectos { get; set; }
    public DbSet<Tarea> Tareas { get; set; }
    public DbSet<Sesion> Sesiones { get; set; }
    public DbSet<PomodoroSesion> PomodoroSesiones { get; set; }
    public DbSet<SesionTrabajo> SesionesTrabajo { get; set; }
    public DbSet<SesionDescanso> SesionesDescanso { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración Usuario
        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Proyectos)
            .WithOne(p => p.Usuario)
            .HasForeignKey(p => p.UsuarioId);

        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Sesiones)
            .WithOne(s => s.Usuario)
            .HasForeignKey(s => s.UsuarioId);

        // Configuración Proyecto
        modelBuilder.Entity<Proyecto>()
            .HasMany(p => p.Tareas)
            .WithOne(t => t.Proyecto)
            .HasForeignKey(t => t.ProyectoId);

        // Configuración Tarea
        modelBuilder.Entity<Tarea>()
            .HasMany(t => t.Sesiones)
            .WithOne(s => s.Tarea)
            .HasForeignKey(s => s.TareaId);

        // Configuración Sesion
        modelBuilder.Entity<Sesion>()
            .HasOne(s => s.SesionTrabajo)
            .WithOne()
            .HasForeignKey<SesionTrabajo>(st => st.Id);

        modelBuilder.Entity<Sesion>()
            .HasOne(s => s.DescansoCorto)
            .WithOne()
            .HasForeignKey<SesionDescanso>(sd => sd.Id);

        modelBuilder.Entity<Sesion>()
            .HasOne(s => s.DescansoLargo)
            .WithOne()
            .HasForeignKey<SesionDescanso>(sd => sd.Id);

        // Configuración PomodoroSesion (herencia)
        modelBuilder.Entity<PomodoroSesion>()
            .UseTpcMappingStrategy(); // TPC: Una tabla por clase

        modelBuilder.Entity<SesionTrabajo>()
            .ToTable("SesionesTrabajo");

        modelBuilder.Entity<SesionDescanso>()
            .ToTable("SesionesDescanso");

        modelBuilder.Entity<Usuario>()
            .Property(u => u.Nombre)
        .IsRequired()
            .HasMaxLength(32);

        modelBuilder.Entity<Tarea>()
            .Property(t => t.Titulo)
            .IsRequired()
            .HasMaxLength(128);

        //Configuración de enums como texto
        modelBuilder.Entity<Tarea>()
            .Property(t => t.Prioridad)
            .HasConversion(
                v => v.ToString(),
                v => (Prioridad)Enum.Parse(typeof(Prioridad), v))
            .IsRequired();
    }
}