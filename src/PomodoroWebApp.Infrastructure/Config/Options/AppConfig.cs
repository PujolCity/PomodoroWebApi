using Microsoft.Extensions.Configuration;

namespace PomodoroWebApp.Infrastructure.Config.Options;

public class AppConfig
{
    internal SqlServerConfig SqlServerConfig { get; set; }
    internal JwtConfig JwtConfig { get; set; }

    internal AppConfig(IConfiguration configuration)
    {
        SqlServerConfig = configuration.GetSection(nameof(SqlServerConfig)).Get<SqlServerConfig>()!;
        JwtConfig = configuration.GetSection(nameof(JwtConfig)).Get<JwtConfig>()!;
    }
}
