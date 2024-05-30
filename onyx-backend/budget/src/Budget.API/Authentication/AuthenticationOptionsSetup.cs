using Microsoft.Extensions.Options;

namespace Budget.API.Authentication;

public sealed class AuthenticationOptionsSetup : IConfigureOptions<AuthenticationOptions>
{
    private const string sectionName = "Authentication";
    private readonly IConfiguration _configuration;

    public AuthenticationOptionsSetup(IConfiguration configuration)
    {
            _configuration = configuration;
        }

    public void Configure(AuthenticationOptions options)
    {
        _configuration.GetSection(sectionName).Bind(options);

    }
}