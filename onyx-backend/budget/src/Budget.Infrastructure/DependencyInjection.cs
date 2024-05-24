using Budget.Application.Abstractions.Currency;
using Budget.Domain.Accounts;
using Budget.Domain.Categories;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Budget.Infrastructure.CurrencyServices;
using Budget.Infrastructure.CurrencyServices.NbpClient;
using Budget.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedDAL;
using SharedDAL.DataSettings;

namespace Budget.Infrastructure;

public static class DependencyInjection
{
    public static void InjectInfrastructure(this IFunctionsHostBuilder builder)
    {
        builder.Services.AddPersistence();
        builder.Services.AddCurrencyConverter(builder.GetContext().Configuration);
    }

    private static void AddPersistence(this IServiceCollection services)
    {
        services.ConfigureOptions<CosmosDbOptionsSetup>();
        services.AddScoped<CosmosDbContext>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<ISubcategoryRepository, SubcategoryRepository>();
        services.AddScoped<ICounterpartyRepository, CounterpartyRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
    }

    private static void AddCurrencyConverter(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<NbpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["CurrencyConverterBaseUrl"] 
                                         ?? throw new MissingFieldException("Currency converter base url is missing"));
        });

        services.AddTransient<ICurrencyConverter, CurrencyConverter>();
    }
}