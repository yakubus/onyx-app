namespace Budget.Functions.Functions.Categories.Requests;

public sealed record UpdateCategoryRequest
{
    public string NewName { get; set; }
}