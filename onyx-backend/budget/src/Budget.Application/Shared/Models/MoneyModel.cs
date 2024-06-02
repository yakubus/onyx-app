using System.Reflection;
using Models.DataTypes;
using Models.Responses;
using Newtonsoft.Json;

namespace Budget.Application.Shared.Models;

public sealed record MoneyModel
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    /// <summary>
    /// <b>DO NOT USE THIS CONSTRUCTOR!</b><br/>
    /// <i>Only for serialization purposes</i>
    /// </summary>
    [System.Text.Json.Serialization.JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    public MoneyModel(decimal amount, string currency) // Needed for json constructor bug TODO Fix after net8.0 update
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