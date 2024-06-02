using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Budget.API.Authentication;

internal class JwtBearerOptionsSetup : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly AuthenticationOptions _options;

    public JwtBearerOptionsSetup(IOptions<AuthenticationOptions> jwtOptions)
    {
        _options = jwtOptions.Value;
    }

    public void PostConfigure(string? name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidIssuer = _options.Issuer;
        options.TokenValidationParameters.ValidAudience = _options.Audience;
        options.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
    }
}