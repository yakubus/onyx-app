namespace Budget.API.Controllers.Subcategories.Requests;

internal sealed record UpdateSubcategoryRequest
{
    public string NewName { get; set; }
    public string NewDescription { get; set; }
}