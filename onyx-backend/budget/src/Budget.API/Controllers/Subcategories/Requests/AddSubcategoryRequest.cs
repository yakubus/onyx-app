namespace Budget.API.Controllers.Subcategories.Requests;

internal sealed record AddSubcategoryRequest
{
    public Guid ParentSubcategoryId { get; set; }
    public string SubcategoryName { get; set; }
}