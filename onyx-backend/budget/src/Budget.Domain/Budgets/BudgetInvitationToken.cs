using Models.Responses;

namespace Budget.Domain.Budgets;

public sealed record BudgetInvitationToken
{
    public string Value { get; init; }
    public DateTime ExpirationDate { get; init; }
    private const int expirationTimeInMinutes = 60;

    private BudgetInvitationToken(string value, DateTime expirationDate)
    {
        Value = value;
        ExpirationDate = expirationDate;
    }

    internal static BudgetInvitationToken Generate() =>
        new(Guid.NewGuid().ToString(),
            DateTime.UtcNow.AddMinutes(expirationTimeInMinutes));

    internal Result Validate(string token)
    {
        if (ExpirationDate > DateTime.UtcNow)
        {
            return BudgetErrors.TokenExpired;
        }

        var isValid = string.Equals(Value, token, StringComparison.CurrentCultureIgnoreCase);

        return isValid ? 
            Result.Success() : 
            BudgetErrors.InvalidToken;
    }
}