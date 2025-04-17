using Microsoft.Extensions.Configuration;
using PomodoroWebApp.Infrastructure.Config.Options;

namespace PomodoroWebApp.Infrastructure.Config.InitializationExtensions;

/// <summary>
/// Extensiones para cargar configuraciones desde el archivo de configuración.
/// </summary>
public static class ConfigurationLoadExtensions
{
    public static AppConfig GetOptions(this IConfiguration configuration) => new(configuration);

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }
}
