using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Messanger;

internal sealed class MessangerOptionsSetup : IConfigureOptions<MessangerOptions>
{
    private readonly IConfiguration _configuration;

    public MessangerOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(MessangerOptions options)
    {
        _configuration.GetSection("Messanger").Bind(options);
    }
}