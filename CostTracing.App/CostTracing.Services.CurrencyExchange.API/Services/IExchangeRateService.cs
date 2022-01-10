using CostTracing.Backend.CurrencyExchange.API.Services.Data;
using CostTracing.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services
{
    public interface ICurrencyExchangeService
    {
        Task<IEnumerable<CurrencyConversionResult>> ConvertCurrencyAsync(double amount, string baseCurrency, params string[] targetCurrencySymbols);

        Task<IEnumerable<Currency>> GetAvailableCurrenciesAsync();
    }
}
