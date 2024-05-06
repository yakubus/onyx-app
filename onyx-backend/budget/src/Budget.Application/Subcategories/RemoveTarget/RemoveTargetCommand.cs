using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Budget.Application.Subcategories.Models;

namespace Budget.Application.Subcategories.RemoveTarget;

public sealed record RemoveTargetCommand(Guid SubcategoryId) : ICommand
{
}