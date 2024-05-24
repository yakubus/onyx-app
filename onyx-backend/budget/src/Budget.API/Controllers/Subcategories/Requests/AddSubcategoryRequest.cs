namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record AddSubcategoryRequest
{
    public Guid ParentCategoryId { get; set; }
    public string SubcategoryName { get; set; }

    private AddSubcategoryRequest(Guid parentCategoryId, string subcategoryName)
    {
        ParentCategoryId = parentCategoryId;
        SubcategoryName = subcategoryName;
    }
}