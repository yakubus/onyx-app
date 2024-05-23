using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Identity.Application.Abstractions.Authentication;
using Identity.Domain;
using Identity.Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Models.Responses;

namespace Identity.Infrastructure.Authentication;

internal sealed class JwtService : IJwtService
{
    private static readonly Error AuthenticationFailedError = new(
        "Jwt.AuthenticationFailedError",
        "Failed to acquire access token do to authentication failure");
    private static readonly Error TokenCreationFailedError = new(
        "Jwt.CreationFailure",
        "Failed to generate access token");

    private readonly AuthenticationOptions _options;

    public JwtService(IOptions<AuthenticationOptions> options)
    {
        _options = options.Value;
    }

    public Result<string> GenerateJwt(User user)
    {
        var claims = UserRepresentationModel
            .FromUser(user)
            .ToClaims();

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddMinutes(_options.ExpireInMinutes),
            signingCredentials);

        var tokenValue = new JwtSecurityTokenHandler()
            .WriteToken(token);

        return tokenValue is null ?
                Result.Failure<string>(AuthenticationFailedError) :
                Result.Success(tokenValue);
    }

    public Result<string> GetUserIdFromToken(string encodedToken)
    {
        try
        {
            var jwt = new JsonWebToken(encodedToken);

            jwt.TryGetPayloadValue(UserRepresentationModel.IdClaimName, out string userId);

            return userId ?? Result.Failure<string>(TokenCreationFailedError);
        }
        catch (Exception)
        {
            return Result.Failure<string>(TokenCreationFailedError);
        }
    }
}