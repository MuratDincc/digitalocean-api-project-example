using Microsoft.AspNetCore.Mvc;
using System.Xml;
using Test.Api.Models.Currency;

namespace Test.Api.Controllers;

[ApiController]
[Route("api/v1/currencies")]
public class CurrencyController : ControllerBase
{
    private readonly ILogger<CurrencyController> _logger;

    public CurrencyController(ILogger<CurrencyController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(GetCurrenciesResponse), 200)]
    public async Task<IActionResult> Get()
    {
        var response = new GetCurrenciesResponse()
        {
            Currencies = new()
        };

        try
        {
            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");

            decimal usd = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "USD")).InnerText.Replace('.', ','));
            decimal eur = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "EUR")).InnerText.Replace('.', ','));
            decimal gbp = Convert.ToDecimal(xmlVerisi.SelectSingleNode(string.Format("Tarih_Date/Currency[@Kod='{0}']/ForexSelling", "GBP")).InnerText.Replace('.', ','));

            response.Currencies.Add(new GetCurrencyResponse
            {
                Code = "USD",
                SalePrice = usd
            });

            response.Currencies.Add(new GetCurrencyResponse
            {
                Code = "EUR",
                SalePrice = eur
            });

            response.Currencies.Add(new GetCurrencyResponse
            {
                Code = "GBP",
                SalePrice = gbp
            });
        }
        catch (XmlException xml)
        {
            return BadRequest();
        }

        return Ok(response);
    }
}