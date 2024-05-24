using Abstractions.DomainBaseTypes;
using Converters.DateTime;
using Models.DataTypes;
using Models.Responses;
using Newtonsoft.Json;

namespace Identity.Domain;

public sealed class User : Entity<UserId>
{
    public Email Email { get; private set; }
    public Username Username { get; private set; }
    public Password PasswordHash { get; private set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? LastLoggedInAt { get; private set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime RegisteredAt { get; init; }
    //TODO Convert to Refresh Tokens
    public bool IsAuthenticated { get; private set; }
    public Currency Currency { get; private set; }
    public bool IsEmailVerified { get; private set; }
    public LoggingGuard Guard { get; init; }
    public bool IsPasswordForgotten { get; private set; }
    public bool IsEmailChangeRequested { get; private set; }
    public VerificationCode? VerificationCode { get; private set; }

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
        bool isPasswordForgotten,
        bool isEmailChangeRequested,
        VerificationCode? verificationCode,
        LoggingGuard guard,
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
        IsPasswordForgotten = isPasswordForgotten;
        IsEmailChangeRequested = isEmailChangeRequested;
        VerificationCode = verificationCode;
        Guard = guard;
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
            false,
            false,
            false,
            null,
            LoggingGuard.Create());
    }

    public Result LogIn(string passwordPlainText)
    {
        if (Guard.IsLocked)
        {
            return UserErrors.UserLocked(Guard.RemainingLockSeconds);
        }

        var passwordVerifyResult = PasswordHash.VerifyPassword(passwordPlainText);

        if (passwordVerifyResult.IsFailure)
        {
            Guard.LogInFailed();
            return Result.Failure(passwordVerifyResult.Error);
        }

        IsAuthenticated = true;
        LastLoggedInAt = DateTime.UtcNow;
        Guard.LoginSucceeded();

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

    public Result ChangePassword(string passwordPlainText, string verificationCode)
    {
        var verificationCodeResult = VerifyCode(verificationCode);

        if (verificationCodeResult.IsFailure)
        {
            return verificationCodeResult.Error;
        }

        if (!IsPasswordForgotten)
        {
            return UserErrors.PasswordChangeNotRequested;
        }

        var passwordCreateResult = Password.Create(passwordPlainText);

        if (passwordCreateResult.IsFailure)
        {
            return Result.Failure<User>(passwordCreateResult.Error);
        }

        PasswordHash = passwordCreateResult.Value;

        IsPasswordForgotten = false;

        return Result.Success();
    }

    public Result ChangeEmail(string newEmail, string verificationCode)
    {
        var codeVerificationResult = VerifyCode(verificationCode);

        if (codeVerificationResult.IsFailure)
        {
            return codeVerificationResult.Error;
        }

        if (!IsEmailVerified)
        {
            return UserErrors.EmailNotVerified;
        }

        if (!IsEmailChangeRequested)
        {
            return UserErrors.EmailChangeNotRequested;
        }

        var emailCreateResult = Email.Create(newEmail);

        if (emailCreateResult.IsFailure)
        {
            return Result.Failure<User>(emailCreateResult.Error);
        }

        Email = emailCreateResult.Value;

        IsEmailChangeRequested = false;

        return Result.Success();
    }

    public Result<VerificationCode> RequestEmailChange()
    {
        if (!IsEmailVerified)
        {
            return UserErrors.EmailNotVerified;
        }

        IsEmailChangeRequested = true;

        var verificationCode = GenerateVerificationCode();

        return Result.Success(verificationCode);
    }

    public Result VerifyEmail(string verificationCode)
    {
        var verificationCodeResult = VerifyCode(verificationCode);

        if (verificationCodeResult.IsFailure)
        {
            return verificationCodeResult.Error;
        }

        IsEmailVerified = true;

        return Result.Success();
    }

    public Result<VerificationCode> ForgotPassword()
    {
        if (!IsEmailVerified)
        {
            return UserErrors.EmailNotVerified;
        }

        IsPasswordForgotten = true;

        var verificationCode = GenerateVerificationCode();

        return Result.Success(verificationCode);
    }

    public Result Remove(string passwordPlainText)
    {
        if (!IsAuthenticated)
        {
            return UserErrors.NotLoggedIn;
        }

        var passwordVerifyResult = PasswordHash.VerifyPassword(passwordPlainText);

        if (passwordVerifyResult.IsFailure)
        {
            return passwordVerifyResult.Error;
        }

        return Result.Success();
    }

    private VerificationCode GenerateVerificationCode()
    {
        var code = VerificationCode.Generate();

        VerificationCode = code;

        return code;
    }

    private Result VerifyCode(string code)
    {
        var isSuccess = VerificationCode?.Verify(code) ?? false;

        if (isSuccess)
        {
            VerificationCode = null;
        }

        return isSuccess ?
            Result.Success() :
            Result.Failure(UserErrors.VerificationCodeInvalid);
    }
}