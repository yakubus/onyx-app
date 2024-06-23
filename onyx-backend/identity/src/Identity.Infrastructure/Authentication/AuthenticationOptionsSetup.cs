using Identity.Infrastructure.SecretsManager;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Authentication;

public sealed class AuthenticationOptionsSetup : IConfigureOptions<AuthenticationOptions>
{
    private const string sectionName = "Authentication";
    private readonly IConfiguration _configuration;
    private const string secretName = "onyx/identity/authentication";

    public AuthenticationOptionsSetup(IConfiguration configuration)
    {
            _configuration = configuration;
    }

    public void Configure(AuthenticationOptions options)
    {
        _configuration.GetSection(sectionName).Bind(options);

        if (string.IsNullOrWhiteSpace(options.SecretKey))
        {
            options.SecretKey = SecretAccesor.GetSecret(secretName);
        }
    }
}