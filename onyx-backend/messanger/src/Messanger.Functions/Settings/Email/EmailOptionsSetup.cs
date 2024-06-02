using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Messanger.Functions.Settings.Email;

internal sealed class EmailOptionsSetup : IConfigureOptions<EmailOptions>
{
    private readonly IConfiguration _configuration;
    private const string emailConnectionStringSectionName = "EmailConnectionString";

    public EmailOptionsSetup(IConfiguration configuration)
    {
            _configuration = configuration;
        }

    public void Configure(EmailOptions options)
    {
            options.ConnectionString = _configuration[emailConnectionStringSectionName];
        }
}