using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Budget.Application.Categories.AddCategory;
using Budget.Application.Categories.GetCategories;
using Budget.Application.Categories.RemoveCategory;
using Budget.Application.Categories.UpdateCategory;
using Budget.Functions.Functions.Categories.Requests;
using MediatR;
using Models.Responses;

namespace Budget.Functions.Functions.Categories;

//TODO: Add query for GET to load assignments only for month
public sealed class CategoriesController
{
    private const string baseRoute = "/api/v1/{budgetId}/categories";
    private readonly ISender _sender;

    public CategoriesController(ISender sender) => _sender = sender;

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> GetCategories([FromRoute]Guid budgetId, CancellationToken cancellationToken)
    {
        var query = new GetCategoriesQuery(budgetId);

        var result = await _sender.Send(query, cancellationToken);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> AddCategory(
        [FromRoute] Guid budgetId,
        [FromBody] AddCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddCategoryCommand(request.Name, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{categoryId}}")]
    public async Task<Result> UpdateCategory(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid categoryId,
        [FromBody] UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateCategoryCommand(categoryId, request.NewName, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{categoryId}}")]
    public async Task<Result> RemoveCategory(
        [FromRoute] Guid budgetId,
        [FromRoute] Guid categoryId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveCategoryCommand(categoryId, budgetId);

        var result = await _sender.Send(command, cancellationToken);

        return result;
    }
}