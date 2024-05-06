using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Models.DataTypes;

namespace Budget.Application.Subcategories.RemoveAssignment;

public sealed record RemoveAssignmentCommand(Guid SubcategoryId, MonthDate AssignmentMonth) : ICommand
{
}