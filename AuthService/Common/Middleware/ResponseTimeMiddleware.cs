using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AuthService.Common.Middleware;
public class ResponseTimeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ResponseTimeMiddleware> _logger;

    public ResponseTimeMiddleware(RequestDelegate next, ILogger<ResponseTimeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Request {correlationId} {method} {url} failed",
                context.TraceIdentifier,
                context.Request.Method,
                context.Request.Path
            );

            throw; 
        }
        finally
        {
            stopwatch.Stop();

            var elapsedMs = stopwatch.ElapsedMilliseconds;          

            _logger.LogInformation(
                "Request {correlationId} {method} {url} responded {statusCode} in {elapsed} ms",
                context.TraceIdentifier,
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                elapsedMs
            );
        }
    }
}
