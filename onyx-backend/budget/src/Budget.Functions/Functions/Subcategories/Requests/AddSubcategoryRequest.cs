namespace Budget.Functions.Functions.Subcategories.Requests;

public sealed record AddSubcategoryRequest(Guid ParentCategoryId, string SubcategoryName);