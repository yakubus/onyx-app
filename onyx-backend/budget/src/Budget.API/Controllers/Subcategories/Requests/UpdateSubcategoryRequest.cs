namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record UpdateSubcategoryRequest(string? NewName, string? NewDescription)
{
}