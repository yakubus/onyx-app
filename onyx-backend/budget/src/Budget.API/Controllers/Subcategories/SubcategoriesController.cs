using Budget.API.Controllers.Subcategories.Requests;
using Budget.Application.Subcategories.AddSubcategory;
using Budget.Application.Subcategories.RemoveAssignment;
using Budget.Application.Subcategories.RemoveSubcategory;
using Budget.Application.Subcategories.RemoveTarget;
using Budget.Application.Subcategories.UpdateAssignment;
using Budget.Application.Subcategories.UpdateSubcategory;
using Budget.Application.Subcategories.UpdateTarget;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers.Subcategories;

[ApiController]
[Route("/api/v1/subcategories")]
public sealed class SubcategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public SubcategoriesController(ISender sender) => _sender = sender;

    [HttpPost]
    public async Task<IActionResult> AddSubcategory(
        [FromBody] AddSubcategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddSubcategoryCommand(request.ParentSubcategoryId, request.SubcategoryName);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}")]
    public async Task<IActionResult> UpdateSubcategory(
        [FromRoute] Guid subcategoryId,
        [FromBody] UpdateSubcategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSubcategoryCommand(subcategoryId, request.NewName, request.NewDescription);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{subcategoryId}")]
    public async Task<IActionResult> RemoveSubcategory(
        [FromRoute] Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveSubcategoryCommand(subcategoryId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }


    [HttpPut("{subcategoryId}/assignment")]
    public async Task<IActionResult> UpdateAssignment(
        [FromRoute] Guid subcategoryId,
        [FromBody] UpdateAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAssignmentCommand(subcategoryId, request.AssignmentMonth, request.AssignedAmount);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}/assignment/remove")]
    public async Task<IActionResult> RemoveAssignment(
        [FromRoute] Guid subcategoryId,
        [FromBody] RemoveAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAssignmentCommand(subcategoryId, request.AssignmentMonth);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}/target")]
    public async Task<IActionResult> UpdateTarget(
        [FromRoute] Guid subcategoryId,
        [FromBody] UpdateTargetRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTargetCommand(subcategoryId, request.TargetUpToMonth, request.TargetAmount);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}/target/remove")]
    public async Task<IActionResult> RemoveTarget(
        [FromRoute] Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTargetCommand(subcategoryId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

}