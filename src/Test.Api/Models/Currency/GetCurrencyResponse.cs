namespace Test.Api.Models.Currency;

public record GetCurrencyResponse
{
    public string Code { get; set; }
    public decimal SalePrice { get; set; }
}