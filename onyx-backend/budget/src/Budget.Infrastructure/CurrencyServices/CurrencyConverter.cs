using Budget.Application.Abstractions.Currency;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Infrastructure.CurrencyServices;

internal sealed class CurrencyConverter : ICurrencyConverter
{
    private readonly NbpClient.NbpClient _converterClient;

    public CurrencyConverter(NbpClient.NbpClient converterClient) => 
        _converterClient = converterClient;

    public async Task<Result<Money>> ConvertMoney(Money amount, Currency destinationCurrency, CancellationToken cancellationToken = default)
    {
        var rateGetResult = await _converterClient.GetExchangeRate(
            amount.Currency.Code,
            destinationCurrency.Code,
            cancellationToken);

        if (rateGetResult.IsFailure)
        {
            return Result.Failure<Money>(rateGetResult.Error);
        }

        var rate = rateGetResult.Value;

        var convertedAmount = amount.Amount * rate;

        return new Money(convertedAmount, destinationCurrency);
    }
}