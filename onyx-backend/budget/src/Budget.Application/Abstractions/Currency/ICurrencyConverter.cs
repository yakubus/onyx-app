using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Abstractions.Currency;

public interface ICurrencyConverter
{
    Task<Result<Money>> ConvertMoney(
        Money amount,
        Models.DataTypes.Currency destinationCurrency,
        CancellationToken cancellationToken = default);
}