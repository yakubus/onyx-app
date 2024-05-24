namespace Budget.Infrastructure.CurrencyServices.NbpClient.Models;

internal sealed record NbpResponse
{
    public string Table { get; set; }
    public string Currency { get; set; }
    public string Code { get; set; }
    public List<Rate> Rates { get; set; }

    private NbpResponse(string table, string currency, string code, List<Rate> rates)
    {
        Table = table;
        Currency = currency;
        Code = code;
        Rates = rates;
    }
}

internal sealed record Rate
{
    public string No { get; set; }
    public DateTime EffectiveDate { get; set; }
    public decimal Mid { get; set; }

    private Rate(string no, DateTime effectiveDate, decimal mid)
    {
        No = no;
        EffectiveDate = effectiveDate;
        Mid = mid;
    }
}