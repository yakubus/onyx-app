using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstractions.Messaging;

namespace Budget.Application.Abstractions.BusinessModels;

public abstract record EntityBusinessModel
{
    private readonly List<IDomainEvent> _domainEvents;

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents;

    protected EntityBusinessModel(IEnumerable<IDomainEvent> domainEvents)
    {
        _domainEvents = domainEvents.ToList();
    }
}