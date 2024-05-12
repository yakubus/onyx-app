using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Abstractions.Services;

public interface ICurrencyConverter
{
    Task<Result<Money>> ConvertMoney(Money amount, Currency destinationCurrency, CancellationToken  cancellationToken = default);
}