using Budget.API.Controllers.Categories.Requests;
using Budget.Application.Categories.AddCategory;
using Budget.Application.Categories.GetCategories;
using Budget.Application.Categories.Models;
using Budget.Application.Categories.RemoveCategory;
using Budget.Application.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.API.Controllers.Categories;

//TODO: Add query for GET to load assignments only for month
[ApiController]
[Authorize]
[Route("/api/v1/{budgetId}/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<CategoryModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategories([FromRoute]Guid budgetId, CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery(budgetId);

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Result<CategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(AddCategoryRequest), "application/json")]
    public async Task<IActionResult> AddCategory(
        [FromRoute] Guid budgetId,
        [FromBody] AddCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddCategoryCommand(request.Name, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPut("{categoryId}")]
    [ProducesResponseType(typeof(Result<CategoryModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    [Consumes(typeof(UpdateCategoryRequest), "application/json")]
    public async Task<IActionResult> UpdateCategory(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(categoryId, request.NewName, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpDelete("{categoryId}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> RemoveCategory(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCategoryCommand(categoryId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}