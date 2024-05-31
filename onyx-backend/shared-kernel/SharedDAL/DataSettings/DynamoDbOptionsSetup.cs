using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SharedDAL.DataSettings;

public sealed class DynamoDbOptionsSetup : IConfigureOptions<DynamoDbOptions>
{
    private readonly IConfiguration _configuration;
    private const string sectionName = "DynamoDb";

    public DynamoDbOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DynamoDbOptions options)
    {
        _configuration.GetSection(sectionName).Bind(options);
    }
}