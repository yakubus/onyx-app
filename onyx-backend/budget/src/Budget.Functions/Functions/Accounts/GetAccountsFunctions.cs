using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Budget.Application.Accounts.GetAccounts;
using Budget.Functions.Functions.Shared;
using LambdaKernel;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Accounts;

public sealed class GetAccountsFunctions : BaseFunction
{
    private readonly Regex pathRegex =
        new(@"^/api/v1/budgets/(?<budgetId>[^/]+)(/.*)?$", RegexOptions.Compiled);

    protected override async Task<APIGatewayProxyResponse> HandleAsync(
        APIGatewayProxyRequest functionRequest,
        ILambdaContext context,
        CancellationToken cancellationToken)
    {
        var request = new FunctionRequest(functionRequest);

        var budgetId = request.FromRoute(pathRegex);

        var query = new GetAccountsQuery(Guid.Parse(budgetId));

        var result = await _sender.Send(query, cancellationToken);

        return result.ReturnAPIResponse(200, 404);
    }
}