using Budget.Application.Subcategories.AddSubcategory;
using Budget.Application.Subcategories.RemoveAssignment;
using Budget.Application.Subcategories.RemoveSubcategory;
using Budget.Application.Subcategories.RemoveTarget;
using Budget.Application.Subcategories.UpdateAssignment;
using Budget.Application.Subcategories.UpdateSubcategory;
using Budget.Application.Subcategories.UpdateTarget;
using Budget.Functions.Functions.Subcategories.Requests;
using FunctionsExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Budget.Functions.Functions.Subcategories;

public sealed class SubcategoriesHttpTrigger
{
    private readonly ISender _sender;

    public SubcategoriesHttpTrigger(ISender sender)
    {
        _sender = sender;
    }

    [FunctionName(nameof(AddSubcategory))]
    public async Task<IActionResult> AddSubcategory(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "subcategories")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<AddSubcategoryRequest>(cancellationToken);

        var command = new AddSubcategoryCommand(request.ParentCategoryId, request.SubcategoryName, Guid.Empty);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(UpdateSubcategory))]
    public async Task<IActionResult> UpdateSubcategory(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "subcategories/{subcategoryId}")] HttpRequest req,
        Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<UpdateSubcategoryRequest>(cancellationToken);

        var command = new UpdateSubcategoryCommand(subcategoryId, request.NewName, request.NewDescription, Guid.Empty);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(RemoveSubcategory))]
    public async Task<IActionResult> RemoveSubcategory(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "subcategories/{subcategoryId}")] HttpRequest req,
        Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveSubcategoryCommand(subcategoryId, Guid.Empty);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }


    [FunctionName(nameof(UpdateAssignment))]
    public async Task<IActionResult> UpdateAssignment(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "put",
            Route = "subcategories/{subcategoryId}/assignment")]
        HttpRequest req,
        Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<UpdateAssignmentRequest>(cancellationToken);

        var command = new UpdateAssignmentCommand(subcategoryId, request.AssignmentMonth, request.AssignedAmount, Guid.Empty);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(RemoveAssignment))]
    public async Task<IActionResult> RemoveAssignment(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "put",
            Route = "subcategories/{subcategoryId}/assignment/remove")]
        HttpRequest req,
        Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<RemoveAssignmentRequest>(cancellationToken);

        var command = new RemoveAssignmentCommand(subcategoryId, request.AssignmentMonth, Guid.Empty);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(UpdateTarget))]
    public async Task<IActionResult> UpdateTarget(
        [HttpTrigger(
            AuthorizationLevel.Function,
            "put",
            Route = "subcategories/{subcategoryId}/target")]
        HttpRequest req,
        Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var request = await req.Body.ConvertBodyToAsync<UpdateTargetRequest>(cancellationToken);

        var command = new UpdateTargetCommand(subcategoryId, request.TargetUpToMonth, request.TargetAmount, Guid.Empty);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(RemoveTarget))]
    public async Task<IActionResult> RemoveTarget(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "subcategories/{subcategoryId}/target/remove")] HttpRequest req,
        Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTargetCommand(subcategoryId, Guid.Empty);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }
}