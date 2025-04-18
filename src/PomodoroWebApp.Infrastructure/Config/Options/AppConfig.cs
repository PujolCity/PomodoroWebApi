using Microsoft.Extensions.Configuration;

namespace PomodoroWebApp.Infrastructure.Config.Options;

/// <summary>
/// Configuración de la aplicación.
/// </summary>
public class AppConfig
{
    internal SqlServerConfig SqlServerConfig { get; set; }
    internal JwtConfig JwtConfig { get; set; }
    internal SerilogConfig SerilogConfig { get; set; }

    internal AppConfig(IConfiguration configuration)
    {
        SqlServerConfig = configuration.GetSection(nameof(SqlServerConfig)).Get<SqlServerConfig>()!;
        JwtConfig = configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>()!;
        SerilogConfig = configuration.GetSection(nameof(SerilogConfig)).Get<SerilogConfig>()!;
    }
}
