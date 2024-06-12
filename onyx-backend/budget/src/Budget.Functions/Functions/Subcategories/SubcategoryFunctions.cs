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
        Guid budgetId,
        [FromQuery] int month,
        [FromQuery] int year)
    {
        var command = new GetToAssignQuery(month, year);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> AddSubcategory(
        Guid budgetId,
        [FromBody] AddSubcategoryRequest request)
    {
        var command = new AddSubcategoryCommand(request.ParentCategoryId, request.SubcategoryName, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}")]
    public async Task<Result> UpdateSubcategory(
        Guid budgetId,
        Guid subcategoryId,
        [FromBody] UpdateSubcategoryRequest request)
    {
        var command = new UpdateSubcategoryCommand(subcategoryId, request.NewName, request.NewDescription, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{subcategoryId}}")]
    public async Task<Result> RemoveSubcategory(
        Guid budgetId,
        Guid subcategoryId)
    {
        var command = new RemoveSubcategoryCommand(subcategoryId, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/assignment")]
    public async Task<Result> UpdateAssignment(
        Guid budgetId,
        Guid subcategoryId,
        [FromBody] UpdateAssignmentRequest request)
    {
        var command = new UpdateAssignmentCommand(subcategoryId, request.AssignmentMonth, request.AssignedAmount, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/assignment/remove")]
    public async Task<Result> RemoveAssignment(
        Guid budgetId,
        Guid subcategoryId,
        [FromBody] RemoveAssignmentRequest request)
    {
        var command = new RemoveAssignmentCommand(subcategoryId, request.AssignmentMonth, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/target")]
    public async Task<Result> UpdateTarget(
        Guid budgetId,
        Guid subcategoryId,
        [FromBody] UpdateTargetRequest request)
    {
        var command = new UpdateTargetCommand(subcategoryId, request.StartedAt, request.TargetUpToMonth, request.TargetAmount, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{subcategoryId}}/target/remove")]
    public async Task<Result> RemoveTarget(
        Guid budgetId,
        Guid subcategoryId)
    {
        var command = new RemoveTargetCommand(subcategoryId, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

}