namespace Budget.API.Controllers.Counterparties.Requests;

internal sealed record AddCounterpartyRequest
{
    public string CounterpartyType { get; set; }
    public string CounterpartyName { get; set;}
}