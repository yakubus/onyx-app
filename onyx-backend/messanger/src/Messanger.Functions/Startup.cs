using Messanger.Functions;
using Messanger.Functions.Services.Emails;
using Messanger.Functions.Settings.Email;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

[assembly: FunctionsStartup(typeof(Startup))]

namespace Messanger.Functions
{
    public sealed class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.ConfigureOptions<EmailOptionsSetup>();
            builder.Services.AddTransient<EmailService>();
        }

        public override void ConfigureAppConfiguration(IFunctionsConfigurationBuilder builder)
        {
            base.ConfigureAppConfiguration(builder);

            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("host.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            AddSecrets(builder);
        }

        private static IConfigurationBuilder AddSecrets(IFunctionsConfigurationBuilder builder) =>
            builder.GetContext().EnvironmentName switch
            {
                var envName when envName == EnvironmentName.Development => AddDevelopmentSecrets(builder),
                //var envName when envName == Environments.Production => AddProductionSecrets(builder),
                _ => builder.ConfigurationBuilder
            };
        // No need for now
        //private static IConfigurationBuilder AddProductionSecrets(
        //    IFunctionsConfigurationBuilder builder)
        //{
        //    var vaultUri = new Uri(builder.ConfigurationBuilder.Build()["KeyVaultUri"] ??
        //                           throw new ConfigurationErrorsException("Missing KeyVaultUri"));

        //    return builder.ConfigurationBuilder
        //        .AddAzureKeyVault(vaultUri, new DefaultAzureCredential())
        //        .AddEnvironmentVariables();
        //}

        private static IConfigurationBuilder AddDevelopmentSecrets(
            IFunctionsConfigurationBuilder builder) =>
            builder.ConfigurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
    }
}
