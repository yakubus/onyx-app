using System.Reflection;
using Amazon.DynamoDBv2.DocumentModel;
using Identity.Domain;
using Models.DataTypes;
using SharedDAL.DataModels;
using SharedDAL.DataModels.Abstractions;
using SharedDAL.Extensions;

namespace Identity.Infrastructure.Data.DataModels;

internal class UserDataModel : IDataModel<User>
{
    public Guid Id { get; init; }
    public string Username { get; init; }
    public string Email { get; init; }
    public string PasswordHash { get; init; }
    public int LastLoggedInAtDay { get; init; }
    public int LastLoggedInAtMonth { get; init; }
    public int LastLoggedInAtYear { get; init; }
    public int LastLoggedInAtHour { get; init; }
    public int LastLoggedInAtMinute { get; init; }
    public int LastLoggedInAtSecond { get; init; }
    public int RegisteredAtDay { get; init; }
    public int RegisteredAtMonth { get; init; }
    public int RegisteredAtYear { get; init; }
    public int RegisteredAtHour { get; init; }
    public int RegisteredAtMinute { get; init; }
    public int RegisteredAtSecond { get; init; }
    public string? LongLivedToken { get; init; }
    public string Currency { get; init; }
    public bool IsEmailVerified { get; init; }
    public int GuardLoginAttempts { get; init; }
    public int? GuardLockedUntilDay { get; init; }
    public int? GuardLockedUntilMonth { get; init; }
    public int? GuardLockedUntilYear { get; init; }
    public int? GuardLockedUntilHour { get; init; }
    public int? GuardLockedUntilMinute { get; init; }
    public int? GuardLockedUntilSecond { get; init; }
    public bool IsPasswordForgotten { get; init; }
    public bool IsEmailChangeRequested { get; init; }
    public string? VerificationCodeCode { get; init; }
    public int? VerificationCodeExpirationDateDay { get; init; }
    public int? VerificationCodeExpirationDateMonth { get; init; }
    public int? VerificationCodeExpirationDateYear { get; init; }
    public int? VerificationCodeExpirationDateHour { get; init; }
    public int? VerificationCodeExpirationDateMinute { get; init; }
    public int? VerificationCodeExpirationDateSecond { get; init; }

    public UserDataModel(Document doc)
    {
        Id = doc[nameof(Id)].AsGuid();
        Username = doc[nameof(Username)];
        Email = doc[nameof(Email)];
        PasswordHash = doc[nameof(PasswordHash)];
        LastLoggedInAtDay = doc[nameof(LastLoggedInAtDay)].AsInt();
        LastLoggedInAtMonth = doc[nameof(LastLoggedInAtMonth)].AsInt();
        LastLoggedInAtYear = doc[nameof(LastLoggedInAtYear)].AsInt();
        LastLoggedInAtHour = doc[nameof(LastLoggedInAtHour)].AsInt();
        LastLoggedInAtMinute = doc[nameof(LastLoggedInAtMinute)].AsInt();
        LastLoggedInAtSecond = doc[nameof(LastLoggedInAtSecond)].AsInt();
        RegisteredAtDay = doc[nameof(RegisteredAtDay)].AsInt();
        RegisteredAtMonth = doc[nameof(RegisteredAtMonth)].AsInt();
        RegisteredAtYear = doc[nameof(RegisteredAtYear)].AsInt();
        RegisteredAtHour = doc[nameof(RegisteredAtHour)].AsInt();
        RegisteredAtMinute = doc[nameof(RegisteredAtMinute)].AsInt();
        RegisteredAtSecond = doc[nameof(RegisteredAtSecond)].AsInt();
        LongLivedToken = doc[nameof(LongLivedToken)].AsNullableString();
        Currency = doc[nameof(Currency)];
        IsEmailVerified = doc[nameof(IsEmailVerified)].AsBoolean();
        GuardLoginAttempts = doc[nameof(GuardLoginAttempts)].AsInt();
        GuardLockedUntilDay = doc[nameof(GuardLockedUntilDay)].AsNullableInt();
        GuardLockedUntilMonth = doc[nameof(GuardLockedUntilMonth)].AsNullableInt();
        GuardLockedUntilYear = doc[nameof(GuardLockedUntilYear)].AsNullableInt();
        GuardLockedUntilHour = doc[nameof(GuardLockedUntilHour)].AsNullableInt();
        GuardLockedUntilMinute = doc[nameof(GuardLockedUntilMinute)].AsNullableInt();
        GuardLockedUntilSecond = doc[nameof(GuardLockedUntilSecond)].AsNullableInt();
        IsPasswordForgotten = doc[nameof(IsPasswordForgotten)].AsBoolean();
        IsEmailChangeRequested = doc[nameof(IsEmailChangeRequested)].AsBoolean();
        VerificationCodeCode = doc[nameof(VerificationCodeCode)].AsNullableString();
        VerificationCodeExpirationDateDay = doc[nameof(VerificationCodeExpirationDateDay)].AsNullableInt();
        VerificationCodeExpirationDateMonth = doc[nameof(VerificationCodeExpirationDateMonth)].AsNullableInt();
        VerificationCodeExpirationDateYear = doc[nameof(VerificationCodeExpirationDateYear)].AsNullableInt();
        VerificationCodeExpirationDateHour = doc[nameof(VerificationCodeExpirationDateHour)].AsNullableInt();
        VerificationCodeExpirationDateMinute = doc[nameof(VerificationCodeExpirationDateMinute)].AsNullableInt();
        VerificationCodeExpirationDateSecond = doc[nameof(VerificationCodeExpirationDateSecond)].AsNullableInt();
    }


    public UserDataModel(User user)
    {
        Id = user.Id.Value;
        Username = user.Username.Value;
        Email = user.Email.Value;
        PasswordHash = user.PasswordHash.Hash;
        LastLoggedInAtDay = user.LastLoggedInAt.Day;
        LastLoggedInAtMonth = user.LastLoggedInAt.Month;
        LastLoggedInAtYear = user.LastLoggedInAt.Year;
        LastLoggedInAtHour = user.LastLoggedInAt.Hour;
        LastLoggedInAtMinute = user.LastLoggedInAt.Minute;
        LastLoggedInAtSecond = user.LastLoggedInAt.Second;
        RegisteredAtDay = user.RegisteredAt.Day;
        RegisteredAtMonth = user.RegisteredAt.Month;
        RegisteredAtYear = user.RegisteredAt.Year;
        RegisteredAtHour = user.RegisteredAt.Hour;
        RegisteredAtMinute = user.RegisteredAt.Minute;
        RegisteredAtSecond = user.RegisteredAt.Second;
        LongLivedToken = user.LongLivedToken;
        Currency = user.Currency.Code;
        IsEmailVerified = user.IsEmailVerified;
        GuardLoginAttempts = user.Guard.LoginAttempts;
        GuardLockedUntilDay = user.Guard.LockedUntil?.Day;
        GuardLockedUntilMonth = user.Guard.LockedUntil?.Month;
        GuardLockedUntilYear = user.Guard.LockedUntil?.Year;
        GuardLockedUntilHour = user.Guard.LockedUntil?.Hour;
        GuardLockedUntilMinute = user.Guard.LockedUntil?.Minute;
        GuardLockedUntilSecond = user.Guard.LockedUntil?.Second;
        IsPasswordForgotten = user.IsPasswordForgotten;
        IsEmailChangeRequested = user.IsEmailChangeRequested;
        VerificationCodeCode = user.VerificationCode?.Code;
        VerificationCodeExpirationDateDay = user.VerificationCode?.ExpirationDate.Day;
        VerificationCodeExpirationDateMonth = user.VerificationCode?.ExpirationDate.Month;
        VerificationCodeExpirationDateYear = user.VerificationCode?.ExpirationDate.Year;
        VerificationCodeExpirationDateHour = user.VerificationCode?.ExpirationDate.Hour;
        VerificationCodeExpirationDateMinute = user.VerificationCode?.ExpirationDate.Minute;
        VerificationCodeExpirationDateSecond = user.VerificationCode?.ExpirationDate.Second;
    }

    public Type GetDomainModelType() => typeof(User);

    public static UserDataModel FromDomainModel(User domainModel) => new (domainModel);

    public static UserDataModel FromDocument(Document doc) => new(doc);

    public User ToDomainModel()
    {
        var id = new UserId(Id);
        var lastLoggedInAt = new DateTime(
            LastLoggedInAtYear,   
            LastLoggedInAtMonth,
            LastLoggedInAtDay,
            LastLoggedInAtHour,
            LastLoggedInAtMinute,
            LastLoggedInAtSecond,
            DateTimeKind.Utc);
        var registeredAt = new DateTime(
            RegisteredAtYear,
            RegisteredAtMonth,
            RegisteredAtDay,
            RegisteredAtHour,
            RegisteredAtMinute,
            RegisteredAtSecond,
            DateTimeKind.Utc);
        DateTime? guardLockedUntilDay = 
            GuardLockedUntilDay is null ||
            GuardLockedUntilMonth is null ||
            GuardLockedUntilYear is null ||
            GuardLockedUntilHour is null ||
            GuardLockedUntilMinute is null ||
            GuardLockedUntilSecond is null
                ? null
                : new DateTime(
                    GuardLockedUntilYear.Value,
                    GuardLockedUntilMonth.Value,
                    GuardLockedUntilDay.Value,
                    GuardLockedUntilHour.Value,
                    GuardLockedUntilMinute.Value,
                    GuardLockedUntilSecond.Value,
                    DateTimeKind.Utc);
        DateTime? verificationCodeExpirationDate =
            VerificationCodeExpirationDateDay is null ||
            VerificationCodeExpirationDateMonth is null ||
            VerificationCodeExpirationDateYear is null ||
            VerificationCodeExpirationDateHour is null ||
            VerificationCodeExpirationDateMinute is null ||
            VerificationCodeExpirationDateSecond is null ?
                null :
                new DateTime(
                    VerificationCodeExpirationDateYear.Value,
                    VerificationCodeExpirationDateMonth.Value,
                    VerificationCodeExpirationDateDay.Value,
                    VerificationCodeExpirationDateHour.Value,
                    VerificationCodeExpirationDateMinute.Value,
                    VerificationCodeExpirationDateSecond.Value,
                    DateTimeKind.Utc);

        var username = Activator.CreateInstance(
                           typeof(Username),
                           BindingFlags.Instance | BindingFlags.NonPublic,
                           null,
                           [Username],
                           null) as Username ??
                       throw new DataModelConversionException(
                           typeof(string),
                           typeof(Username),
                           typeof(UserDataModel));

        var email = Activator.CreateInstance(
                        typeof(Domain.Email),
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        [Email],
                        null) as Domain.Email ??
                    throw new DataModelConversionException(
                        typeof(string),
                        typeof(Domain.Email),
                        typeof(UserDataModel));

        var passwordHash = Activator.CreateInstance(
                               typeof(Password),
                               BindingFlags.Instance | BindingFlags.NonPublic,
                               null,
                               [PasswordHash],
                               null) as Password ??
                           throw new DataModelConversionException(
                               typeof(string),
                               typeof(Password),
                               typeof(UserDataModel));

        var currency = Activator.CreateInstance(
                           typeof(Currency),
                           BindingFlags.Instance | BindingFlags.NonPublic,
                           null,
                           [Currency],
                           null) as Currency ??
                       throw new DataModelConversionException(
                           typeof(string),
                           typeof(Currency),
                           typeof(UserDataModel));

        var guard = Activator.CreateInstance(
                        typeof(LoggingGuard),
                        BindingFlags.Instance | BindingFlags.NonPublic,
                        null,
                        [GuardLoginAttempts, guardLockedUntilDay],
                        null) as LoggingGuard ??
                    throw new DataModelConversionException(
                        typeof(string),
                        typeof(LoggingGuard),
                        typeof(UserDataModel));

        var verificationCode = verificationCodeExpirationDate is null || VerificationCodeCode is null ?
            null :
            Activator.CreateInstance(
                typeof(VerificationCode),
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                [VerificationCodeCode, verificationCodeExpirationDate],
                null) as VerificationCode ??
            throw new DataModelConversionException(
                typeof(string),
                typeof(VerificationCode),
                typeof(UserDataModel));

        var user =
            Activator.CreateInstance(
                typeof(User),
                BindingFlags.Instance | BindingFlags.NonPublic,
                null,
                [
                    email,
                    username,
                    passwordHash,
                    lastLoggedInAt,
                    registeredAt,
                    currency,
                    IsEmailVerified,
                    IsPasswordForgotten,
                    IsEmailChangeRequested,
                    verificationCode,
                    guard,
                    LongLivedToken,
                    id
                ],
                null) as User ??
            throw new DataModelConversionException(
                typeof(UserDataModel),
                typeof(User));

        return user;
    }
}