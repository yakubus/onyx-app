using Models.Responses;

namespace Budget.Domain.Subcategories
{
    internal static class SubcategoryErrors
    {
        internal static readonly Error SubcategoryAlreadyAssignedForMonth = new(
            "Subcategory.AlreadyAssignedForMonth",
            "Subcategory is already assigned for this month");

        internal static readonly Error InvalidNameError = new(
            "Subcategory.Message.InvalidValue",
            "Invalid subcategory name input");

        internal static readonly Error DescriptionTooLong = new(
            "Subcategory.Description.DescriptionTooLong",
            "Subcategory description input is too long");

        internal static readonly Error AssignmentAmountMustBePositive = new(
            "Subcategory.Assignment.Amount.MustBePositive",
            "Subcategory assignment amount must be positive");

        internal static readonly Error AssignmentDateMustBeInNextOrCurrentMonth = new(
            "Subcategory.Assignment.Date.MustBeInNextorCurrentMonth",
            "Subcategory assignment date must be in next or current month");

        internal static readonly Error SubcategoryNotAssignedForMonth = new(
            "Subcategory.NotAssignedForMonth",
            "Subcategory is not assigned for this month");

        internal static readonly Error WrongTransactionDateTimeForAssignment = new(
            "Subcategory.Assignment.WrongTransactionDateTime",
            "Assignment does not cover date of transaction");

        internal static readonly Error TargetAmountMustBePositive = new(
            "Subcategory.Target.Amount.MustBePositive",
            "Target amount must be positive");

        internal static readonly Error TargetDateMustBeInFuture = new(
            "Subcategory.Target.EndDate.MustBeInNextorCurrentMonth",
            "Target date must be in future month");

        internal static readonly Error TargetDateHasPassed = new (
            "Subcategory.Target.EndDate.HasPassed",
            "Target date has passed");
        
        internal static Error TargetStartedAfterTransactionDate = new (
            "Subcategory.Target.StartDate.IsAfterTransactionDate",
            "Target start date is after transaction date");
    }
}
