using Budget.Application.Shared.Models;
using Budget.Application.Subcategories.AddSubcategory;
using Budget.Application.Subcategories.GetToAssign;
using Budget.Application.Subcategories.Models;
using Budget.Application.Subcategories.RemoveAssignment;
using Budget.Application.Subcategories.RemoveSubcategory;
using Budget.Application.Subcategories.RemoveTarget;
using Budget.Application.Subcategories.UpdateAssignment;
using Budget.Application.Subcategories.UpdateSubcategory;
using Budget.Application.Subcategories.UpdateTarget;
using Budget.Functions.Functions.Subcategories.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.Functions.Functions.Subcategories;

[ApiController]
[Authorize]
[Route("/api/v1/{budgetId}/subcategories")]
public sealed class SubcategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public SubcategoriesController(ISender sender) => _sender = sender;

    [HttpGet("to-assign")]
    [ProducesResponseType(typeof(Result<MoneyModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetToAssignAmount(
        [FromQuery] int month,
        [FromQuery] int year,
        CancellationToken cancellationToken)
    {
        var command = new GetToAssignQuery(month, year);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<SubcategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(AddSubcategoryRequest), "application/json")]
    public async Task<IActionResult> AddSubcategory(
        [FromRoute] Guid budgetId,
        [FromBody] AddSubcategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddSubcategoryCommand(request.ParentCategoryId, request.SubcategoryName, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}")]
    [ProducesResponseType(typeof(Result<SubcategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateSubcategoryRequest), "application/json")]
    public async Task<IActionResult> UpdateSubcategory(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid subcategoryId,
        [FromBody] UpdateSubcategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSubcategoryCommand(subcategoryId, request.NewName, request.NewDescription, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{subcategoryId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveSubcategory(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveSubcategoryCommand(subcategoryId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }


    [HttpPut("{subcategoryId}/assignment")]
    [ProducesResponseType(typeof(Result<SubcategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateAssignmentRequest), "application/json")]
    public async Task<IActionResult> UpdateAssignment(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid subcategoryId,
        [FromBody] UpdateAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAssignmentCommand(subcategoryId, request.AssignmentMonth, request.AssignedAmount, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}/assignment/remove")]
    [ProducesResponseType(typeof(Result<SubcategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(RemoveAssignmentRequest), "application/json")]
    public async Task<IActionResult> RemoveAssignment(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid subcategoryId,
        [FromBody] RemoveAssignmentRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAssignmentCommand(subcategoryId, request.AssignmentMonth, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}/target")]
    [ProducesResponseType(typeof(Result<SubcategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateTargetRequest), "application/json")]
    public async Task<IActionResult> UpdateTarget(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid subcategoryId,
        [FromBody] UpdateTargetRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateTargetCommand(subcategoryId, request.StartedAt, request.TargetUpToMonth, request.TargetAmount, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{subcategoryId}/target/remove")]
    [ProducesResponseType(typeof(Result<SubcategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveTarget(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid subcategoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveTargetCommand(subcategoryId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

}