using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PomodoroWebApp.Extensions;

public class OnlyInEnvironmentAttribute : ActionFilterAttribute
{
    private readonly string _targetEnvironment;
    
    public OnlyInEnvironmentAttribute(string targetEnvironment)
    {
        _targetEnvironment = targetEnvironment;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var env = context.HttpContext.RequestServices.GetService<IWebHostEnvironment>();

        if (env == null || !string.Equals(env.EnvironmentName, _targetEnvironment, StringComparison.OrdinalIgnoreCase))
        {
            context.Result = new NotFoundResult(); 
        }
    }
}
