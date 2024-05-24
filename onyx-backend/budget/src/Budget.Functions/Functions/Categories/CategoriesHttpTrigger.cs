using System;
using System.Threading;
using System.Threading.Tasks;
using Budget.Application.Categories.AddCategory;
using Budget.Application.Categories.GetCategories;
using Budget.Application.Categories.RemoveCategory;
using Budget.Application.Categories.UpdateCategory;
using Budget.Functions.Functions.Categories.Requests;
using FunctionsExtensions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Budget.Functions.Functions.Categories;

public sealed class CategoriesHttpTrigger
{
    private readonly ISender _sender;

    public CategoriesHttpTrigger(ISender sender)
    {
        _sender = sender;
    }

    [FunctionName(nameof(GetCategories))]
    public async Task<IActionResult> GetCategories(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "categories")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery();

        var result = await _sender.Send(query, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(AddCategory))]
    public async Task<IActionResult> AddCategory(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "categories")] HttpRequest req,
        CancellationToken cancellationToken)
    {
        var request = await req.ConvertBodyToAsync<AddCategoryRequest>(cancellationToken);

        var command = new AddCategoryCommand(request.Name);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(UpdateCategory))]
    public async Task<IActionResult> UpdateCategory(
        [HttpTrigger(AuthorizationLevel.Function, "put", Route = "categories/{categoryId}")] HttpRequest req,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var request = await req.ConvertBodyToAsync<UpdateCategoryRequest>(cancellationToken);

        var command = new UpdateCategoryCommand(categoryId, request.NewName);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }

    [FunctionName(nameof(RemoveCategory))]
    public async Task<IActionResult> RemoveCategory(
        [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "categories/{categoryId}")] HttpRequest req,
        Guid categoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCategoryCommand(categoryId);

        var result = await _sender.Send(command, cancellationToken);

        return result.IsSuccess ?
            new OkObjectResult(result) :
            new BadRequestObjectResult(result);
    }
}