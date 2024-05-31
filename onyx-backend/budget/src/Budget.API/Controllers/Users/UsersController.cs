using Budget.API.Controllers.Users.Requests;
using Budget.Application.Users.GetUser;
using Budget.Application.Users.LogIn;
using Budget.Application.Users.Models;
using Budget.Application.Users.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.API.Controllers.Users;

[ApiController]
[Route("api/v1/user")]
public class UsersController : ControllerBase
{
    private readonly ISender _sender;

    public UsersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(Result<UserModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUser(CancellationToken cancellationToken)
    {
        var query = new GetUserQuery();
        
        var result = await _sender.Send(query, cancellationToken);
        
        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost("login")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [Consumes(typeof(LogInRequest), "application/json")]
    public async Task<IActionResult> LogIn(
        [FromBody] LogInRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LogInCommand(request.Email, request.Password);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            Unauthorized(result);
    }


    [HttpPost("register")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [Consumes(typeof(LogInRequest), "application/json")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RegisterCommand(request.Email, request.Password, request.Username, request.Currency);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}