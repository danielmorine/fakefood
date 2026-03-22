using AuthService.Common.Result;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthService.Common;

public static class ResultExecutor
{
    public static async Task<Result<T>> ExecuteAsync<T>(Func<Task<T>> action, string traceId)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            var result = await action();
            stopwatch.Stop();

            return Result<T>.Success(result, stopwatch.Elapsed, traceId);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            return Result<T>.Failure(ex.Message, stopwatch.Elapsed, traceId);
        }
    }

    public static async Task<Result.Base.Result> ExecuteAsync(Func<Task> action, string traceId)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await action();
            stopwatch.Stop();

            return Result.Base.Result.Success(stopwatch.Elapsed, traceId);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            return Result.Base.Result.Failure(ex.Message, stopwatch.Elapsed, traceId);
        }
    }
}
