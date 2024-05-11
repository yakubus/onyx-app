using Budget.API.Controllers.Accounts.Requests;
using Budget.API.Controllers.Categories.Requests;
using Budget.Application.Accounts.AddAccount;
using Budget.Application.Accounts.GetAccounts;
using Budget.Application.Accounts.RemoveAccount;
using Budget.Application.Accounts.UpdateAccount;
using Budget.Application.Categories.AddCategory;
using Budget.Application.Categories.GetCategories;
using Budget.Application.Categories.RemoveCategory;
using Budget.Application.Categories.UpdateCategory;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Budget.API.Controllers.Categories;

[ApiController]
[Route("/api/v1/categories")]
public sealed class CategoriesController : ControllerBase
{
    private readonly ISender _sender;

    public CategoriesController(ISender sender) => _sender = sender;

    [HttpGet]
    public async Task<IActionResult> GetCategories(CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            Ok(result) :
            BadRequest(result);
    }

    [HttpPost]
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