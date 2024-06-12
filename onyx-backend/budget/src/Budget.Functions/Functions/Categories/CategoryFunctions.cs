using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Budget.Application.Categories.AddCategory;
using Budget.Application.Categories.GetCategories;
using Budget.Application.Categories.RemoveCategory;
using Budget.Application.Categories.UpdateCategory;
using Budget.Functions.Functions.Categories.Requests;
using MediatR;
using Models.Responses;

//[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace Budget.Functions.Functions.Categories;

//TODO: Add query for GET to load assignments only for month
public sealed class CategoryFunctions
{
    private const string baseRoute = "/api/v1/{budgetId}/categories/";
    private readonly ISender _sender;

    public CategoryFunctions(ISender sender) => _sender = sender;

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> GetCategories(Guid budgetId)
    {
        var query = new GetCategoriesQuery(budgetId);

        var result = await _sender.Send(query);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> AddCategory(
        Guid budgetId,
        [FromBody] AddCategoryRequest request)
    {
        var command = new AddCategoryCommand(request.Name, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{categoryId}}")]
    public async Task<Result> UpdateCategory(
        Guid budgetId,
        Guid categoryId,
        [FromBody] UpdateCategoryRequest request)
    {
        var command = new UpdateCategoryCommand(categoryId, request.NewName, budgetId);

        var result = await _sender.Send(command);

        return result;
    }

    [LambdaFunction()]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{categoryId}}")]
    public async Task<Result> RemoveCategory(
        Guid budgetId,
        Guid categoryId)
    {
        var command = new RemoveCategoryCommand(categoryId, budgetId);

        var result = await _sender.Send(command);

        return result;
    }
}