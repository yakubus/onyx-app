using Budget.Application.Abstractions.Services;
using Budget.Domain.Accounts;
using Budget.Domain.Categories;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Budget.Infrastructure.CurrencyServices;
using Budget.Infrastructure.CurrencyServices.NbpClient;
using Budget.Infrastructure.Data;
using Budget.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Budget.Infrastructure;

public static class DependencyInjection
{
    public static void InjectInfrastructure(this IFunctionsHostBuilder builder)
    {
        builder.Services.AddPersistence(builder.GetContext().Configuration);
    }

    //TODO Temporary solution
    public static IServiceCollection InjectInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence(configuration);
        services.AddCurrencyConverter(configuration);

        return services;
    }

    private static void AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CosmosDbOptions>(configuration.GetSection("CosmosDb"));
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
            client.BaseAddress = new Uri(configuration["CurrencyConverter:BaseUrl"] 
                                         ?? throw new MissingFieldException("Currency converter base url is missing"));
        });

        services.AddTransient<ICurrencyConverter, CurrencyConverter>();
    }
}