namespace Budget.API.Controllers.Categories.Requests;

public sealed record UpdateCategoryRequest
{
    public string NewName { get; set; }
}