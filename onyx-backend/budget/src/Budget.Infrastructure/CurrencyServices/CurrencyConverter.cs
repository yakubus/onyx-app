using Budget.Application.Abstractions.Services;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Infrastructure.CurrencyServices;

internal sealed class CurrencyConverter : ICurrencyConverter
{
    public async Task<Result<Money>> ConvertMoney(Money amount, Currency destinationCurrency, CancellationToken cancellationToken = default)
    {
        return null;
    }
}