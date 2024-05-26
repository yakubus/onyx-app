using System.Text.Json.Serialization;
using Models.DataTypes;
using Models.Responses;

namespace Budget.Application.Shared.Models;

public sealed record MoneyModel
{
    public decimal Amount { get; init; }
    public string Currency { get; init; }

    [JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    internal MoneyModel(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    internal static MoneyModel FromDomainModel(Money money) => new(money.Amount, money.Currency.Code);

    internal Result<Money> ToDomainModel()
    {
        var currencyCreateResult = global::Models.DataTypes.Currency.FromCode(Currency);

        if (currencyCreateResult.IsFailure)
        {
            return Result.Failure<Money>(currencyCreateResult.Error);
        }

        var currency = currencyCreateResult.Value;
        return new Money(Amount, currency);
    }
}