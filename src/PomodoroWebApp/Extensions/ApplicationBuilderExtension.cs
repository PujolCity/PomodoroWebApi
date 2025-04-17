namespace PomodoroWebApp.Extensions;

/// <summary>
/// Extensiones para la configuración del middleware de la aplicación.
/// </summary>
public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder Configure(this IApplicationBuilder app)
    {
        //app.UseHeaderPropagation();
        app.UseSwagger();
        app.UseSwaggerUI();
        //app.AddHealthChecks();
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
        return app;
    }
}
