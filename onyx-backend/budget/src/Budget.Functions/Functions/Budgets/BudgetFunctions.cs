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
using Budget.Functions.Functions.Shared;
using MediatR;
using Models.Responses;



namespace Budget.Functions.Functions.Budgets;

public sealed class BudgetFunctions : BaseFunction
{
    private const string baseRoute = $"{BaseRouteV1}/budgets/";

    public BudgetFunctions(ISender sender) : base(sender)
    {

    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(GetAllBudgets))]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> GetAllBudgets()
    {
        var command = new GetBudgetsQuery();

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(GetAllBudgets))]
    [HttpApi(LambdaHttpMethod.Get, $"{baseRoute}{{budgetId}}")]
    public async Task<Result> GetBudgetDetails(
        string budgetId)
    {
        var command = new GetBudgetDetailsQuery(Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(GetAllBudgets))]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{budgetId}}/invitation")]
    public async Task<Result> GetBudgetInvitation(
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

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(AddBudget))]
    [HttpApi(LambdaHttpMethod.Post, $"{baseRoute}")]
    public async Task<Result> AddBudget([FromBody] AddBudgetRequest request)
    {
        var command = new AddBudgetCommand(request.BudgetName, request.BudgetCurrency);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(RemoveBudgetUser))]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{budgetId}}/remove/{{userId}}")]
    public async Task<Result> RemoveBudgetUser(
        string budgetId,
        string userId)
    {
        var command = new RemoveUserFromBudgetCommand(Guid.Parse(budgetId), userId);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(JoinTheBudget))]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{budgetId}}/join/{{token}}")]
    public async Task<Result> JoinTheBudget(
        string budgetId,
        string token)
    {
        var command = new AddUserToBudgetCommand(Guid.Parse(budgetId), token);

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(RemoveBudget))]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{budgetId}}")]
    public async Task<Result> RemoveBudget(
        string budgetId)
    {
        var command = new RemoveBudgetCommand(Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

}