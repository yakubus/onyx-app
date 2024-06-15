using Amazon.Lambda.Annotations;
using Amazon.Lambda.Annotations.APIGateway;
using Identity.Application.User.GetUser;
using Identity.Application.User.LogoutUser;
using Identity.Application.User.RemoveUser;
using Identity.Application.User.RequestEmailChange;
using Identity.Application.User.UpdateUser;
using Identity.Functions.Controllers.User.Requests;
using Identity.Functions.Functions.Shared;
using MediatR;
using Models.Responses;

namespace Identity.Functions.Functions.User;

public sealed class UserController : BaseFunction
{
    private const string usersBaseRoute = $"{BaseRouteV1}user";

    public UserController(ISender sender) : base(sender)
    {
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(GetUser))]
    [HttpApi(LambdaHttpMethod.Get, $"{usersBaseRoute}")]
    public async Task<Result> GetUser()
    {
        var command = new GetUserQuery();

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(UpdateUser))]
    [HttpApi(LambdaHttpMethod.Put, $"{usersBaseRoute}")]
    public async Task<Result> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var command = new UpdateUserCommand(
            request.NewEmail,
            request.NewUsername,
            request.NewPassword,
            request.NewCurrency,
            request.VerificationCode);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(RequestEmailChange))]
    [HttpApi(LambdaHttpMethod.Put, $"{usersBaseRoute}change-email")]
    public async Task<Result> RequestEmailChange()
    {
        var command = new RequestEmailChangeCommand();

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(Logout))]
    [HttpApi(LambdaHttpMethod.Put, $"{usersBaseRoute}logout")]
    public async Task<Result> Logout()
    {
        var command = new LogoutUserCommand();

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(RemoveUser))]
    [HttpApi(LambdaHttpMethod.Delete, $"{usersBaseRoute}remove")]
    public async Task<Result> RemoveUser([FromBody] RemoveUserRequest request)
    {
        var command = new RemoveUserCommand(request.Password);

        var result = await Sender.Send(command);

        return result;
    }
}