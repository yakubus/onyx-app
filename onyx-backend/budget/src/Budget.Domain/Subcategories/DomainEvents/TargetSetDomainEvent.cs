using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;
using Models.DataTypes;

namespace Budget.Domain.Subcategories.DomainEvents;

public sealed record TargetSetDomainEvent(SubcategoryId SubcategoryId) : IDomainEvent
{
}