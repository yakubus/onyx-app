namespace Budget.API.Controllers.Counterparties.Requests;

public sealed record AddCounterpartyRequest
{
    public string CounterpartyType { get; set; }
    public string CounterpartyName { get; set;}

    private AddCounterpartyRequest(string counterpartyType, string counterpartyName)
    {
        CounterpartyType = counterpartyType;
        CounterpartyName = counterpartyName;
    }
}