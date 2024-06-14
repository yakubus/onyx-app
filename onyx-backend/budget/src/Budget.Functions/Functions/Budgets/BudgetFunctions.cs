using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Amazon.Lambda.APIGatewayEvents;
using Budget.Application.Budgets.AddBudget;
using Budget.Application.Budgets.AddUserToBudget;
using Budget.Application.Budgets.GetBudgetDetails;
using Budget.Application.Budgets.GetBudgetInvitation;
using Budget.Application.Budgets.GetBudgets;
using Budget.Application.Budgets.RemoveBudget;
using Budget.Application.Budgets.RemoveUserFromBudgetBudget;
using Budget.Functions.Functions.Budgets.Requests;
using MediatR;
using Models.Responses;

//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Budgets;

public sealed class BudgetFunctions
{
    private const string baseRoute = "/api/v1/budgets/";
    private readonly ISender _sender;

    public BudgetFunctions(ISender sender) => _sender = sender;

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> GetAll()
    {
        var command = new GetBudgetsQuery();

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, $"{baseRoute}{{budgetId}}")]
    public async Task<Result> GetDetails(
        string budgetId)
    {
        var command = new GetBudgetDetailsQuery(Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{budgetId}}/invitation")]
    public async Task<Result> GetInvitation(
        string budgetId,
        APIGatewayHttpApiV2ProxyRequest request)
    {
        var protocol = request.Headers.TryGetValue(
            "X-Forwarded-Proto",
            out var protocolHeader) ?
            protocolHeader :
            "https";
        var host = request.Headers["Host"];
        var path = request.RawPath;
        var baseUrl = $"{protocol}://{host}{path}";
        var command = new GetBudgetInvitationQuery(Guid.Parse(budgetId), baseUrl);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, $"{baseRoute}")]
    public async Task<Result> Add([FromBody] AddBudgetRequest request)
    {
        var command = new AddBudgetCommand(request.BudgetName, request.BudgetCurrency);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{budgetId}}/remove/{{userId}}")]
    public async Task<Result> RemoveUser(
        string budgetId,
        string userId)
    {
        var command = new RemoveUserFromBudgetCommand(Guid.Parse(budgetId), userId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{budgetId}}/join/{{token}}")]
    public async Task<Result> Join(
        string budgetId,
        string token)
    {
        var command = new AddUserToBudgetCommand(Guid.Parse(budgetId), token);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{budgetId}}")]
    public async Task<Result> Remove(
        string budgetId)
    {
        var command = new RemoveBudgetCommand(Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

}