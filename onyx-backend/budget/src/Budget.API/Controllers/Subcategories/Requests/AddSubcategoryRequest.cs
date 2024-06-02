namespace Budget.API.Controllers.Subcategories.Requests;

public sealed record AddSubcategoryRequest(Guid ParentCategoryId, string SubcategoryName);