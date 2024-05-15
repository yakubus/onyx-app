using Abstractions.DomainBaseTypes;
using Models.DataTypes;
using Models.Responses;
using Newtonsoft.Json;

namespace Identity.Domain;

public sealed class User : Entity<UserId>
{
    public Email Email { get; private set; }
    public Username Username { get; private set; }
    public Password PasswordHash { get; private set; }
    public DateTime? LastLoggedInAt { get; private set; }
    public DateTime RegisteredAt { get; init; }
    public bool IsAuthenticated { get; private set; }
    public Currency Currency { get; private set; }
    public bool IsEmailVerified { get; private set; }

    [JsonConstructor]
    [System.Text.Json.Serialization.JsonConstructor]
    private User(
        Email email,
        Username username,
        Password passwordHash,
        DateTime? lastLoggedInAt,
        DateTime registeredAt,
        bool isAuthenticated,
        Currency currency,
        bool isEmailVerified,
        UserId? userId = null) : base(userId ?? new UserId())
    {
        Email = email;
        Username = username;
        PasswordHash = passwordHash;
        LastLoggedInAt = lastLoggedInAt;
        RegisteredAt = registeredAt;
        IsAuthenticated = isAuthenticated;
        Currency = currency;
        IsEmailVerified = isEmailVerified;
    }

    public static Result<User> Register(
        string email,
        string username, 
        string passwordPlainText,
        string currency)
    {
        var emailCreateResult = Email.Create(email);

        if (emailCreateResult.IsFailure)
        {
            return Result.Failure<User>(emailCreateResult.Error);
        }

        var usernameCreateResult = Username.Create(username);

        if (usernameCreateResult.IsFailure)
        {
            return Result.Failure<User>(usernameCreateResult.Error);
        }

        var passwordCreateResult = Password.Create(passwordPlainText);

        if (passwordCreateResult.IsFailure)
        {
            return Result.Failure<User>(passwordCreateResult.Error);
        }

        var currencyCreateResult = Models.DataTypes.Currency.FromCode(currency);

        if (currencyCreateResult.IsFailure)
        {
            return Result.Failure<User>(currencyCreateResult.Error);
        }

        return new User(
            emailCreateResult.Value,
            usernameCreateResult.Value,
            passwordCreateResult.Value,
            null,
            DateTime.UtcNow,
            false,
            currencyCreateResult.Value,
            false);
    }

    public Result LogIn(string passwordPlainText)
    {
        var passwordVerifyResult = PasswordHash.VerifyPassword(passwordPlainText);

        if (passwordVerifyResult.IsFailure)
        {
            return Result.Failure(passwordVerifyResult.Error);
        }

        IsAuthenticated = true;
        LastLoggedInAt = DateTime.UtcNow;

        return Result.Success();
    }

    public Result LogOut()
    {
        IsAuthenticated = false;

        return Result.Success();
    }

    public Result ChangeCurrency(string newCurrency)
    {
        var currencyCreateResult = Currency.FromCode(newCurrency);

        if (currencyCreateResult.IsFailure)
        {
            return Result.Failure(currencyCreateResult.Error);
        }

        Currency = currencyCreateResult.Value;

        return Result.Success();
    }

    public Result ChangeUsername(string username)
    {
        var usernameCreateResult = Username.Create(username);

        if (usernameCreateResult.IsFailure)
        {
            return Result.Failure(usernameCreateResult.Error);
        }

        Username = usernameCreateResult.Value;

        return Result.Success();
    }

    public Result ChangePassword(string passwordPlainText)
    {
        var passwordCreateResult = Password.Create(passwordPlainText);

        if (passwordCreateResult.IsFailure)
        {
            return Result.Failure<User>(passwordCreateResult.Error);
        }

        PasswordHash = passwordCreateResult.Value;

        return Result.Success();
    }

    public Result ChangeEmail(string newEmail)
    {
        var emailCreateResult = Email.Create(newEmail);

        if (emailCreateResult.IsFailure)
        {
            return Result.Failure<User>(emailCreateResult.Error);
        }

        Email = emailCreateResult.Value;

        return Result.Success();
    }
}