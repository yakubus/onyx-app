using Budget.Application.Abstractions.Currency;
using Budget.Application.Abstractions.Identity;
using Budget.Domain.Accounts;
using Budget.Domain.Budgets;
using Budget.Domain.Categories;
using Budget.Domain.Counterparties;
using Budget.Domain.Subcategories;
using Budget.Domain.Transactions;
using Budget.Infrastructure.Contexts;
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
    public static IFunctionsHostBuilder InjectInfrastructure(this IFunctionsHostBuilder builder)
    {
        builder.Services.AddPersistence();
        builder.Services.AddCurrencyConverter(builder.GetContext().Configuration);
        builder.Services.AddContexts();

        return builder;
    }

    public static void InjectInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPersistence();
        services.AddCurrencyConverter(configuration);
        services.AddContexts();
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
        services.AddScoped<IBudgetRepository, BudgetRepository>();
    }

    private static void AddCurrencyConverter(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<NbpClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["CurrencyConverterBaseUrl"] 
                                         ?? throw new MissingFieldException("BalanceCurrency converter base url is missing"));
        });

        services.AddTransient<ICurrencyConverter, CurrencyConverter>();
    }

    private static void AddContexts(this IServiceCollection services)
    {
        //TODO consider
        services.AddHttpContextAccessor();

        services.AddScoped<IBudgetContext, BudgetContext>();
        services.AddScoped<IUserContext, UserContext>();
    }
}