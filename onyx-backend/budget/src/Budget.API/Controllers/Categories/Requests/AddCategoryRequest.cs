namespace Budget.API.Controllers.Categories.Requests;

internal sealed record AddCategoryRequest
{
    public string Name { get; set; }
}