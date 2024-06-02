namespace Budget.Functions.Functions.Subcategories.Requests;

public sealed record AddSubcategoryRequest
{
    public Guid ParentCategoryId { get; set; }
    public string SubcategoryName { get; set; }
}