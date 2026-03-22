using AuthService.Presentation.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService.Common.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
            stopwatch.Stop();

            if (context.Response.HasStarted)
            {
                _logger.LogWarning("Resposta já iniciada, não é possível tratar erro.");
                throw;
            }

            var traceId = context.TraceIdentifier;

            _logger.LogError(ex, "Erro não tratado | TraceId: {traceId}", traceId);

            var response = new ApiResult
            {
                IsSuccess = false,
                Error = ex.Message,
                TraceId = traceId,
                ExecutionTimeMs = stopwatch.Elapsed.TotalMilliseconds
            };

            var json = JsonSerializer.Serialize(response);

            await context.Response.WriteAsync(json);
        }
    }
}