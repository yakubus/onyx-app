using Models.Exceptions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.DataTypes;

public record Money(
    [property: BsonRepresentation(BsonType.Decimal128)]
    decimal Amount, Currency Currency)
{
    // Overrides

    public override string ToString() => $"{Amount} {Currency.Code}";

    // Operators

    public static Money operator +(Money first, decimal secondAmount) =>
        first with { Amount = first.Amount + secondAmount };

    public static Money operator -(Money first, decimal secondAmount) =>
        first with { Amount = first.Amount - secondAmount };

    public static Money operator -(Money first) =>
        first with { Amount = -first.Amount };

    public static Money operator /(Money first, decimal secondAmount) =>
        first with { Amount = first.Amount / secondAmount };


    public static Money operator *(Money first, decimal secondAmount) =>
        first with { Amount = first.Amount * secondAmount };

    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first with { Amount = first.Amount + second.Amount };
    }

    public static Money operator -(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first with { Amount = first.Amount - second.Amount };
    }

    public static Money operator *(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first with { Amount = first.Amount * second.Amount };
    }

    public static Money operator /(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first with { Amount = first.Amount / second.Amount };
    }

    public static bool operator <(Money first, decimal secondAmount) =>
        first.Amount < secondAmount;

    public static bool operator <=(Money first, decimal secondAmount) =>
        first < secondAmount || first == secondAmount;

    public static bool operator >(Money first, decimal secondAmount) =>
        first.Amount > secondAmount;

    public static bool operator >=(Money first, decimal secondAmount) =>
        first > secondAmount || first == secondAmount;


    public static bool operator ==(Money first, decimal secondAmount) =>
        first.Amount == secondAmount;

    public static bool operator !=(Money first, decimal secondAmount) =>
        !(first == secondAmount);

    public static bool operator <(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first < second.Amount;
    }

    public static bool operator <=(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first < second.Amount || first == second.Amount;
    }

    public static bool operator >=(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first > second.Amount || first == second.Amount;
    }

    public static bool operator >(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new DomainException<Money>("Currencies must be equal");
        }

        return first > second.Amount;
    }
}