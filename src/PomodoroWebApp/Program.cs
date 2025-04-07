using FluentValidation;
using PomodoroWebApp.Application.AppModule;
using PomodoroWebApp.Extensions;
using PomodoroWebApp.Infrastructure.Config.Extensions;
using PomodoroWebApp.Infrastructure.Config.InitializationExtensions;

var builder = WebApplication.CreateBuilder(args);

var configurationBuilder = new ConfigurationBuilder()
              .SetBasePath(AppContext.BaseDirectory)
              .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: true)
.AddEnvironmentVariables();

//var entorno = builder.Environment.EnvironmentName;
//Console.WriteLine($"entorno : {entorno}");

var services = builder.Services;

IConfiguration configuration = configurationBuilder.Build();
services.ConfigureInfrastructure(configuration);
configuration.GetOptions();
builder.Services.AddUserValidators();
var app = builder.Build();
app.Configure();
app.Run();
