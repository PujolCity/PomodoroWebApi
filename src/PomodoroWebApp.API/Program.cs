using PomodoroWebApp.Extensions;
using PomodoroWebApp.Infrastructure.Extensions;
using PomodoroWebApp.Infrastructure.InitializationExtensions;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
.AddEnvironmentVariables();

var entorno = builder.Environment.EnvironmentName;
Console.WriteLine($"entorno : {entorno}");

var services = builder.Services;

IConfiguration configuration = configurationBuilder.Build();
services.ConfigureInfrastructure(configuration);
configuration.GetOptions();

var app = builder.Build();
app.Configure();
app.Run();
