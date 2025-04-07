namespace PomodoroWebApp.Domain.Results;

public interface IResult<out TValue> : IResultBase
{
    //
    // Summary:
    //     Get the Value. If result is failed then an Exception is thrown because a failed
    //     result has no value. Opposite see property ValueOrDefault.
    TValue Value { get; }
}