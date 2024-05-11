namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record AddSubcategoryRequest
{
    public Guid ParentSubcategoryId { get; set; }
    public string SubcategoryName { get; set; }
}