namespace Test.Api.Models.Currency;

public record GetCurrenciesResponse
{
    public List<GetCurrencyResponse> Currencies { get; set; }
}