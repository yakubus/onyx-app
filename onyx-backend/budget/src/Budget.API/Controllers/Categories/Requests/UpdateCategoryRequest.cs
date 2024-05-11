namespace Budget.API.Controllers.Categories.Requests;

internal sealed record UpdateCategoryRequest
{
    public string NewName { get; set; }
}