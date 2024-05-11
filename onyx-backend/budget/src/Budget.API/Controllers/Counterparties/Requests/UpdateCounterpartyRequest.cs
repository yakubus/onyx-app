namespace Budget.API.Controllers.Counterparties.Requests;

internal sealed record UpdateCounterpartyRequest
{
    public string NewName { get; set; }
}