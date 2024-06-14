using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Budget.Application.Subcategories.AddSubcategory;
using Budget.Application.Subcategories.GetToAssign;
using Budget.Application.Subcategories.RemoveAssignment;
using Budget.Application.Subcategories.RemoveSubcategory;
using Budget.Application.Subcategories.RemoveTarget;
using Budget.Application.Subcategories.UpdateAssignment;
using Budget.Application.Subcategories.UpdateSubcategory;
using Budget.Application.Subcategories.UpdateTarget;
using Budget.Functions.Functions.Subcategories.Requests;
using MediatR;
using Models.Responses;

//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Subcategories;

public sealed class SubcategoryFunctions
{
    private const string baseRoute = "/api/v1/{budgetId}/subcategories/";
    private readonly ISender _sender;

    public SubcategoryFunctions(ISender sender) => _sender = sender;

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, $"{baseRoute}to-assign")]
    public async Task<Result> GetToAssignAmount(
        string budgetId,
        [FromQuery] int month,
        [FromQuery] int year)
    {
        var command = new GetToAssignQuery(month, year);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> Add(
        string budgetId,
        [FromBody] AddSubcategoryRequest request)
    {
        var command = new AddSubcategoryCommand(
            request.ParentCategoryId,
            request.SubcategoryName,
            Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}")]
    public async Task<Result> Update(
        string budgetId,
        string subcategoryId,
        [FromBody] UpdateSubcategoryRequest request)
    {
        var command = new UpdateSubcategoryCommand(
            Guid.Parse(subcategoryId),
            request.NewName,
            request.NewDescription,
            Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{subcategoryId}}")]
    public async Task<Result> Remove(
        string budgetId,
        string subcategoryId)
    {
        var command = new RemoveSubcategoryCommand(Guid.Parse(subcategoryId), Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/assignment")]
    public async Task<Result> UpdateAssignment(
        string budgetId,
        string subcategoryId,
        [FromBody] UpdateAssignmentRequest request)
    {
        var command = new UpdateAssignmentCommand(
            Guid.Parse(subcategoryId),
            request.AssignmentMonth,
            request.AssignedAmount,
            Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/assignment/remove")]
    public async Task<Result> RemoveAssignment(
        string budgetId,
        string subcategoryId,
        [FromBody] RemoveAssignmentRequest request)
    {
        var command = new RemoveAssignmentCommand(
            Guid.Parse(subcategoryId),
            request.AssignmentMonth,
            Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/target")]
    public async Task<Result> UpdateTarget(
        string budgetId,
        string subcategoryId,
        [FromBody] UpdateTargetRequest request)
    {
        var command = new UpdateTargetCommand(
            Guid.Parse(subcategoryId),
            request.StartedAt,
            request.TargetUpToMonth,
            request.TargetAmount,
            Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/target/remove")]
    public async Task<Result> RemoveTarget(
        string budgetId,
        string subcategoryId)
    {
        var command = new RemoveTargetCommand(Guid.Parse(subcategoryId), Guid.Parse(budgetId));

        var result = await _sender.Send(command);

        return result;
    }

}