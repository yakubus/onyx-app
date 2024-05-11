namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record UpdateSubcategoryRequest
{
    public string NewName { get; set; }
    public string NewDescription { get; set; }
}