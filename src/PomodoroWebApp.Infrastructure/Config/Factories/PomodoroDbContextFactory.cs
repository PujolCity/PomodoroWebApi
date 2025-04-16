using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using PomodoroWebApp.Infrastructure.Data;

namespace PomodoroWebApp.Infrastructure.Config.Factories;

public class PomodoroDbContextFactory : IDesignTimeDbContextFactory<PomodoroDbContext>
{
    public PomodoroDbContext CreateDbContext(string[] args)
    {
        var environment = /*Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? */"DESARROLLO";
        Console.WriteLine($"ASPNETCORE_ENVIRONMENT String: {environment}");

        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<PomodoroDbContext>();
        var connectionString = configuration["SqlServerConfig:DefaultConnection"];
        //Console.WriteLine($"Connection String: {connectionString}");
        optionsBuilder.UseSqlServer(connectionString);

        return new PomodoroDbContext(optionsBuilder.Options);
    }
}
