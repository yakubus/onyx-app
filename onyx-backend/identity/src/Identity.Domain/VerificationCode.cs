using System.Text;
using Converters.DateTime;
using Newtonsoft.Json;

namespace Identity.Domain;

public sealed record VerificationCode
{
    public string Code { get; init; }
    [JsonConverter(typeof(DateTimeConverter))]
    public DateTime ExpirationDate { get; init; }

    private VerificationCode(string code, DateTime expirationDate)
    {
        Code = code;
        ExpirationDate = expirationDate;
    }

    internal static VerificationCode Generate()
    {
        var rng = new Random();
        var codeBuilder = new StringBuilder();

        for (int i = 0; i <= 5; i++)
        {
            codeBuilder.Append(rng.Next(0, 10));
        }

        return new VerificationCode(
            codeBuilder.ToString(),
            DateTime.UtcNow.AddMinutes(5));
    }

    internal bool Verify(string code)
    {
        if (DateTime.UtcNow > ExpirationDate)
        {
            return false;
        }

        return Code == code;
    }
}