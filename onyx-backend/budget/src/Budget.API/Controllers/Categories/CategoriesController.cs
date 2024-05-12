using Budget.API.Controllers.Categories.Requests;
using Budget.Application.Categories.AddCategory;
using Budget.Application.Categories.GetCategories;
using Budget.Application.Categories.Models;
using Budget.Application.Categories.RemoveCategory;
using Budget.Application.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Models.Responses;

namespace Budget.API.Controllers.Categories;

[ApiController]
[Route("/api/v1/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender) => _sender = sender;

    [HttpGet]
    [ProducesResponseType(typeof(Result<IEnumerable<CategoryModel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery();

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
        [FromBody] AddCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddCategoryCommand(request.Name);

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
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(categoryId, request.NewName);

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
    [EndpointDescription("Deletes category and all related subcategories (not removes transactions)")]
    public async Task<IActionResult> RemoveCategory(
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCategoryCommand(categoryId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }
}