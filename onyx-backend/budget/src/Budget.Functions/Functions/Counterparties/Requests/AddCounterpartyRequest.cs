namespace Budget.Functions.Functions.Counterparties.Requests;

public sealed record AddCounterpartyRequest
{
    public string CounterpartyType { get; set; }
    public string CounterpartyName { get; set;}
}