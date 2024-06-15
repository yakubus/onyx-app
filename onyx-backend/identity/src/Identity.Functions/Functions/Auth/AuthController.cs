using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Identity.Application.Auth.ForgotPassword;
using Identity.Application.Auth.LoginUser;
using Identity.Application.Auth.RefreshAccessToken;
using Identity.Application.Auth.RegisterUser;
using Identity.Application.Auth.ResendEmail;
using Identity.Application.Auth.VerifyEmail;
using Identity.Functions.Controllers.Auth.Requests;
using Identity.Functions.Functions.Shared;
using MediatR;
using Models.Responses;

namespace Identity.Functions.Functions.Auth;

public sealed class AuthController : BaseFunction
{
    private const string authBaseRoute = $"{BaseRouteV1}auth/";

    public AuthController(ISender sender) : base(sender)
    {
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(Login))]
    [HttpApi(LambdaHttpMethod.Post, $"{authBaseRoute}login")]
    public async Task<Result> Login(
        [FromBody] LoginRequest request)
    {
        var command = new LoginUserCommand(request.Email, request.Password);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(Register))]
    [HttpApi(LambdaHttpMethod.Post, $"{authBaseRoute}register")]
    public async Task<Result> Register(
        [FromBody] RegisterRequest request)
    {
        var command = new RegisterUserCommand(request.Email, request.Username, request.Password, request.Currency);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(VerifyEmail))]
    [HttpApi(LambdaHttpMethod.Put, $"{authBaseRoute}verify-email")]
    public async Task<Result> VerifyEmail(
        [FromBody] VerifyEmailRequest request)
    {
        var command = new VerifyEmailCommand(request.Email, request.VerificationCode);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(ResendEmail))]
    [HttpApi(LambdaHttpMethod.Put, $"{authBaseRoute}resend-email")]
    public async Task<Result> ResendEmail(
        [FromBody] ResendEmailRequest request)
    {
        var command = new ResendEmailCommand(request.Email, request.MessageType);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(Refresh))]
    [HttpApi(LambdaHttpMethod.Put, $"{authBaseRoute}refresh")]
    public async Task<Result> Refresh(
        [FromBody] RefreshRequest request,
        [FromHeader(Name = "Authorization")] string expiredToken)
    {
        var command = new RefreshAccessTokenCommand(request.LongLivedToken, expiredToken);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(Register))]
    [HttpApi(LambdaHttpMethod.Put, $"{authBaseRoute}forgot-password")]
    public async Task<Result> ForgotPassword(
        [FromBody] ForgotPasswordRequest request)
    {
        var command = new ForgotPasswordCommand(request.Email);

        var result = await Sender.Send(command);

        return result;
    }
}