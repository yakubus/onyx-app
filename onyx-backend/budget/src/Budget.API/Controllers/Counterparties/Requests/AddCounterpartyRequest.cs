namespace Budget.API.Controllers.Counterparties.Requests;

public sealed record AddCounterpartyRequest(string CounterpartyType, string CounterpartyName);