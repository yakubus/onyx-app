using Budget.Infrastructure.CurrencyServices.NbpClient.Models;
using Models.Responses;
using Newtonsoft.Json;

namespace Budget.Infrastructure.CurrencyServices.NbpClient;

internal sealed class NbpClient
{
    private readonly HttpClient _httpClient;
    private readonly Error _nbpResponseError = new(
        "NbpClient.Error",
        "Problem while converting currency");

    public NbpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<decimal>> GetExchangeRate(
        string fromCurrency,
        string toCurrency,
        CancellationToken cancellationToken)
    {
        var fromCurrencyPlnRateGetResult = await GetPlnExchange(fromCurrency, cancellationToken);
        var toCurrencyPlnRateGetResult = await GetPlnExchange(toCurrency, cancellationToken);

        if (fromCurrencyPlnRateGetResult.IsFailure || toCurrencyPlnRateGetResult.IsFailure)
        {
            return Result.Failure<decimal>(fromCurrencyPlnRateGetResult.Error);
        }

        var fromCurrencyPlnRate = fromCurrencyPlnRateGetResult.Value;
        var toCurrencyPlnRate = toCurrencyPlnRateGetResult.Value;

        return fromCurrencyPlnRate / toCurrencyPlnRate;
    }

    private async Task<Result<decimal>> GetPlnExchange(string currency, CancellationToken cancellationToken)
    {
        var response = await _httpClient.SendAsync(new (HttpMethod.Get, currency), cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return Result.Failure<decimal>(_nbpResponseError);
        }

        var plainTextResponse = await response.Content.ReadAsStringAsync(cancellationToken);
        var model = JsonConvert.DeserializeObject<NbpResponse>(plainTextResponse);

        var rate = model?.Rates[0].Mid;

        return rate ?? Result.Failure<decimal>(_nbpResponseError);
    }
}