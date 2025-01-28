using Microsoft.Extensions.Configuration;
using PomodoroWebApp.Infrastructure.Options;

namespace PomodoroWebApp.Infrastructure.InitializationExtensions;


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
