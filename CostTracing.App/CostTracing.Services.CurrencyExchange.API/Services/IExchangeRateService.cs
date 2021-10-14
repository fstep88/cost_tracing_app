using CostTracing.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services
{
    public interface ICurrencyExchangeService
    {
        Task<double> ConvertCurrencyAsync(double amount, string baseCurrency, string targetCurrency = "");

        Task<IEnumerable<Currency>> GetAvailableCurrenciesAsync();
    }
}
