using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtTokenBuilder
{
    private string _issuer;
    private string _audience;
    private string _secretKey;
    private DateTime _expires;
    private readonly List<Claim> _claims = new();

    public static JwtTokenBuilder Empty() => new();
    
    public JwtTokenBuilder SetIssuer(string issuer)
    {
        _issuer = issuer;
        return this;
    }

    public JwtTokenBuilder SetAudience(string audience)
    {
        _audience = audience;
        return this;
    }

    public JwtTokenBuilder SetSecretKey(string secretKey)
    {
        _secretKey = secretKey;
        return this;
    }

    public JwtTokenBuilder SetExpiration(TimeSpan duration)
    {
        _expires = DateTime.UtcNow.Add(duration);
        return this;
    }

    public JwtTokenBuilder AddClaim(string type, string value)
    {
        _claims.Add(new Claim(type, value));
        return this;
    }

    public string Build()
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: _claims,
            expires: _expires,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
