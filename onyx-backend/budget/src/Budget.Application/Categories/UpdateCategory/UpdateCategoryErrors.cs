using Models.Responses;

namespace Budget.Application.Categories.UpdateCategory;

internal sealed class UpdateCategoryErrors
{
    internal static readonly Error CategoryAlreadyExistsError = new(
        "Category.AlreadyExists",
        "Category already exists");
}