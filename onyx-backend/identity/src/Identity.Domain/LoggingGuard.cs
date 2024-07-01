using Converters.DateTime;
using Models.Responses;
using Newtonsoft.Json;

namespace Identity.Domain;

public sealed record LoggingGuard
{
    public int LoginAttempts { get; private set; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime? LockedUntil { get; private set; }
    internal bool IsLocked => DateTime.UtcNow < LockedUntil;
    internal int RemainingLockSeconds => LockedUntil is null ? 
        0 :
        (int)(LockedUntil - DateTime.UtcNow).Value.TotalSeconds;

    private static readonly Dictionary<int, int> userLockPolicy = new()
    {
        { 5, 1 },
        { 10, 2 },
        { 15, 3 },
    };

    private LoggingGuard(int loginAttempts, DateTime? lockedUntil)
    {
        LoginAttempts = loginAttempts;
        LockedUntil = lockedUntil;
    }

    internal static LoggingGuard Create() => new(0, null);

    internal Result LogInFailed()
    {
        LoginAttempts++;

        switch (LoginAttempts)
        {
            case var x when x % 5 == 0 && x > 10:
                LockedUntil = DateTime.UtcNow.AddMinutes(userLockPolicy[15]);
                return UserErrors.UserLocked(userLockPolicy[15] * 60);
            case 10:
                LockedUntil = DateTime.UtcNow.AddMinutes(userLockPolicy[10]);
                return UserErrors.UserLocked(userLockPolicy[10] * 60);
            case 5:
                LockedUntil = DateTime.UtcNow.AddMinutes(userLockPolicy[5]);
                return UserErrors.UserLocked(userLockPolicy[5] * 60);
        }

        return Result.Success();
    }

    internal void LoginSucceeded() => (LoginAttempts, LockedUntil) = (0, null);
}