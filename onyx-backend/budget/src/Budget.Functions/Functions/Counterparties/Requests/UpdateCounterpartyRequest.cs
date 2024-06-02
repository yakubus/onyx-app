namespace Budget.Functions.Functions.Counterparties.Requests;

public sealed record UpdateCounterpartyRequest
{
    public string NewName { get; set; }
}