using System.Text.Json.Serialization;
using Abstractions.Messaging;
using Budget.Domain.Counterparties;

namespace Budget.Application.Counterparties.Models;

public sealed record CounterpartyModel : EntityBusinessModel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Type { get; init; }

    [JsonConstructor]
    private CounterpartyModel(Guid id, string name, string type, IEnumerable<IDomainEvent> domainEvents) : base(domainEvents)
    {
        Id = id;
        Name = name;
        Type = type;
    }


    internal static CounterpartyModel FromDomainModel(Counterparty domainModel) =>
        new(domainModel.Id.Value,
            domainModel.Name.Value,
            domainModel.Type.Value,
            domainModel.GetDomainEvents());
}