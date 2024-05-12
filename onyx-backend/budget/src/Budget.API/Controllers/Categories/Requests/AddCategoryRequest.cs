namespace Budget.API.Controllers.Categories.Requests;

public sealed record AddCategoryRequest
{
    public string Name { get; set; }
}