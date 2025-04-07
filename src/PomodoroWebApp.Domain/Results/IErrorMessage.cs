namespace PomodoroWebApp.Domain.Results;

public interface IErrorMessage
{
    string Code { get; set; }

    string Message { get; set; }
}