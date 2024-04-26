using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Responses;

namespace Budget.Domain.Subcategories
{
    internal static class SubcategoryErrors
    {
        internal static readonly Error InvalidNameError = new(
            "Subcategory.Name.InvalidValue",
            "Invalid subcategory name input");

        internal static readonly Error DescriptionTooLong = new(
            "Subcategory.Description.DescriptionTooLong",
            "Subcategory description input is too long");

        internal static readonly Error AssignmentAmountMustBePositive = new(
            "Subcategory.Assignment.Amount.MustBePositive",
            "Subcategory assignment amount must be positive");

        internal static readonly Error AssignmentDateMustBeInNextorCurrentMonth = new(
            "Subcategory.Assignment.Date.MustBeInNextorCurrentMonth",
            "Subcategory assignment date must be in next or current month");
    }
}
