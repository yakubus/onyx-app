using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Models.Responses;

namespace Identity.Domain;

public sealed record Password
{
    private const byte keySize = 32;
    private const byte saltSize = 16;
    private const int iterations = 3_000_000;
    private static readonly HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;
    private const char delimiter = ';';
    private const string passwordPattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$";

    public string Hash { get; init; }

    private Password(string hash)
    {
        Hash = hash;
    }

    internal static Result<Password> Create(string plainTextPassword)
    {
        if (!Regex.IsMatch(plainTextPassword, passwordPattern))
        {
            return Result.Failure<Password>(UserErrors.PasswordTooWeak);
        }

        var salt = RandomNumberGenerator.GetBytes(saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(plainTextPassword),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        return new Password(
            string.Join(
                delimiter, 
                Convert.ToBase64String(salt), 
                Convert.ToBase64String(hash)));
    }


    internal Result VerifyPassword(string passwordPlainText)
    {
        var elements = Hash.Split(delimiter);
        var salt = Convert.FromBase64String(elements[0]);
        var hash = Convert.FromBase64String(elements[1]);

        var hashInput = Rfc2898DeriveBytes.Pbkdf2(
            Hash,
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        var isValid = CryptographicOperations.FixedTimeEquals(hash, hashInput);

        return isValid ?
            Result.Success() :
            Result.Failure(UserErrors.InvalidCredentials); 
    }
}