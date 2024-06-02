using System.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace SharedDAL.DataSettings;

public sealed class CosmosDbOptionsSetup : IConfigureOptions<CosmosDbOptions>
{
    private const string primaryKeyFieldName = "CosmosDb-PrimaryKey";
    private readonly IConfiguration _configuration;

    public CosmosDbOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(CosmosDbOptions options)
    {
        _configuration.GetSection("CosmosDb").Bind(options);

        if (string.IsNullOrWhiteSpace(options.PrimaryKey))
        {
            options.PrimaryKey = _configuration[primaryKeyFieldName] ?? 
                                 throw new ConfigurationErrorsException("CosmosDB primary key is missing");
        }
    }
}