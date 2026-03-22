using AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System;

namespace AuthService.Infrastructure.Correlation;

public class HttpCorrelationIdProvider : ICorrelationIdProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HttpCorrelationIdProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string Get()
    {
        return _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();
    }
}
