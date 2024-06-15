using Amazon.Lambda.Annotations.APIGateway;
using Amazon.Lambda.Annotations;
using Budget.Application.Categories.AddCategory;
using Budget.Application.Categories.GetCategories;
using Budget.Application.Categories.RemoveCategory;
using Budget.Application.Categories.UpdateCategory;
using Budget.Functions.Functions.Categories.Requests;
using Budget.Functions.Functions.Shared;
using MediatR;
using Models.Responses;



namespace Budget.Functions.Functions.Categories;

//TODO: Add query for GET to load assignments only for month
public sealed class CategoryFunctions : BaseFunction
{
    private const string baseRoute = $"{BaseRouteV1}{{budgetId}}/categories/";

    public CategoryFunctions(ISender sender) : base(sender)
    {
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(GetAllCategories))]
    [HttpApi(LambdaHttpMethod.Get, baseRoute)]
    public async Task<Result> GetAllCategories(string budgetId)
    {
        var query = new GetCategoriesQuery(Guid.Parse(budgetId));

        var result = await Sender.Send(query);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(AddCategory))]
    [HttpApi(LambdaHttpMethod.Post, baseRoute)]
    public async Task<Result> AddCategory(
        string budgetId,
        [FromBody] AddCategoryRequest request)
    {
        var command = new AddCategoryCommand(request.Name, Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(UpdateCategory))]
    [HttpApi(LambdaHttpMethod.Put, $"{baseRoute}{{categoryId}}")]
    public async Task<Result> UpdateCategory(
        string budgetId,
        string categoryId,
        [FromBody] UpdateCategoryRequest request)
    {
        var command = new UpdateCategoryCommand(
            Guid.Parse(categoryId),
            request.NewName,
            Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }

    [LambdaFunction(Role = FullAccessRole, ResourceName = nameof(RemoveCategory))]
    [HttpApi(LambdaHttpMethod.Delete, $"{baseRoute}{{categoryId}}")]
    public async Task<Result> RemoveCategory(
        string budgetId,
        string categoryId)
    {
        var command = new RemoveCategoryCommand(Guid.Parse(categoryId), Guid.Parse(budgetId));

        var result = await Sender.Send(command);

        return result;
    }
}