using System;

namespace AuthService.Common.Result.Base;

public class Result
{
    public bool IsSuccess { get; protected set; }
    public bool IsFailure => !IsSuccess;

    public string Error { get; protected set; }

    public TimeSpan ExecutionTime { get; protected set; }
    public string TraceId { get; protected set; }


    protected Result(bool isSuccess, string error, TimeSpan executionTime, string traceId)
    {
        IsSuccess = isSuccess;
        Error = error;
        ExecutionTime = executionTime;
        TraceId = traceId;
    }

    public static Result Success(TimeSpan executionTime, string traceId)
        => new Result(true, string.Empty, executionTime, traceId);

    public static Result Failure(string error, TimeSpan executionTime, string traceId)
        => new Result(false, error, executionTime, traceId);
}