using Abstractions.Messaging;
using Budget.Application.Shared.Models;
using Budget.Application.Subcategories.Models;
using Models.DataTypes;

namespace Budget.Application.Subcategories.UpdateTarget;

public sealed record UpdateTargetCommand(Guid SubcategoryId, MonthDate TargetUpToMonth, MoneyModel TargetAmount) 
    : ICommand<SubcategoryModel>
{
}