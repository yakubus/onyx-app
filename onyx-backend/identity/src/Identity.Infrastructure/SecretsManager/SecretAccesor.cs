using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace Identity.Infrastructure.SecretsManager;

internal class SecretAccesor
{
    public static string GetSecret(string secretName)
    {
        const string region = "eu-central-1";

        var client = new AmazonSecretsManagerClient(RegionEndpoint.GetBySystemName(region));

        var request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT"
        };

        var response = client.GetSecretValueAsync(request).Result;

        return response.SecretString;
    }
}