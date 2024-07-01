using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Identity.Infrastructure.Email.Options;

internal sealed class EmailOptionsSetup : IConfigureOptions<EmailOptions>
{
    private readonly IConfiguration _configuration;
    private const string topicArnEnvVariableName = "EMAIL_TOPIC_ARN";

    public EmailOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EmailOptions options)
    {
        _configuration.GetSection("Messanger").Bind(options);

        if (string.IsNullOrWhiteSpace(options.TopicArn))
        {
            options.TopicArn = _configuration[topicArnEnvVariableName];
        }
    }
}