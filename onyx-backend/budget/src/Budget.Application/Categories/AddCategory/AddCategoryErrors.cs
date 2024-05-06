using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;

namespace Budget.Application.Categories.AddCategory;

internal sealed class AddCategoryErrors
{
    internal static readonly Error CategoryAlreadyExistsError = new(
        "Category.AlreadyExists",
        "Category already exists");
}