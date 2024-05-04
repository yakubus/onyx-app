using Models.DataTypes;

namespace Budget.Application.Abstractions.Services;

public interface ICurrencyConverter
{
    Task<Money> ConvertMoney(Money amount, Currency destinationCurrency, CancellationToken  cancellationToken = default);
}