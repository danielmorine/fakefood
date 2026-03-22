using System;

namespace AuthService.Common.Result;

public class Result<T> : Base.Result
{
    public T Value { get; private set; }

    private Result(bool isSuccess, T value, string error, TimeSpan executionTime, string traceId)
        : base(isSuccess, error, executionTime, traceId)
    {
        Value = value;
    }

    public static Result<T> Success(T value, TimeSpan executionTime, string traceId)
        => new Result<T>(true, value, string.Empty, executionTime, traceId);

    public static Result<T> Failure(string error, TimeSpan executionTime, string traceId)
        => new Result<T>(false, default!, error, executionTime, traceId);
}
