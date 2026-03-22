using Application.Interfaces;
using AuthService.Infrastructure.Security;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;

namespace Infrastructure.Security;

public class TokenService : ITokenService
{
    private readonly JwtOptions _options;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public string GenerateToken(string email)
    {
        return JwtTokenBuilder
            .Empty()
            .SetIssuer(_options.Issuer)
            .SetAudience(_options.Audience)
            .SetSecretKey(_options.Secret)
            .SetExpiration(TimeSpan.FromHours(_options.ExpirationMinutes))
            .AddClaim(ClaimTypes.Name, email)
            .AddClaim("role", "user")
            .Build();
    }
}
