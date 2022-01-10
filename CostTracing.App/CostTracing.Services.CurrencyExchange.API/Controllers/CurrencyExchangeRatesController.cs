using CostTracing.Backend.CurrencyExchange.API.Services;
using CostTracing.Backend.CurrencyExchange.API.Services.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostTracing.BackEnd.CurrencyExchange.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyExchangeRatesController : ControllerBase
    {

        protected readonly ICurrencyRepository _currencyRepository;
        protected readonly ICurrencyExchangeDataRepository _exchangeDataRepository;
        protected readonly ICurrencyExchangeService _currencyConversionService;

        public CurrencyExchangeRatesController(ICurrencyExchangeService exchangeService, ICurrencyRepository repository, ICurrencyExchangeDataRepository currencyExchangeDataRepository) : base()
        {
            _currencyRepository = repository ?? throw new ArgumentNullException(nameof(repository));
            _currencyConversionService = exchangeService ?? throw new ArgumentNullException(nameof(exchangeService));
            _exchangeDataRepository = currencyExchangeDataRepository ?? throw new ArgumentNullException(nameof(currencyExchangeDataRepository));
        }

        public async Task<IActionResult> GetCurrentExchangeRates([FromQuery] string baseCurrencySymbol)
        {
            try
            {
                baseCurrencySymbol = string.IsNullOrEmpty(baseCurrencySymbol) ? "USD" : baseCurrencySymbol;
                var availableCurrencies = await _currencyConversionService.GetAvailableCurrenciesAsync();

                if (!availableCurrencies.Any(c => c.Symbol.Equals(baseCurrencySymbol)))
                {
                    return this.NotFound($"{baseCurrencySymbol} is not available");
                }

                var convertedRates = await _currencyConversionService.ConvertCurrencyAsync(1, baseCurrencySymbol, availableCurrencies.Select(c => c.Symbol)?.ToArray());
                return Ok(convertedRates);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.ToString());
            }
            catch (Exception ex)
            {
                return this.StatusCode(400, ex.ToString());
            }
        }

        public async Task<IActionResult> GetCurrencies([FromQuery] string id = "")
        {
            try
            {
                var list = string.IsNullOrEmpty(id) ? (object) await _currencyRepository.GetCurrencies() : await _currencyRepository.GetCurrency(id); ;
                return Ok(list);
            }
            catch (ArgumentException ex)
            {
                return this.BadRequest(ex.ToString());
            }
            catch(Exception ex)
            {
                return this.StatusCode(400, ex.ToString());
            }
        }
    }
}
