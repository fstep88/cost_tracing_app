using CostTracing.Backend.CurrencyExchange.API.Options;
using CostTracing.Core.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services.Data
{
    public class CurrencyFileRepository : ICurrencyRepository
    {
        protected string _path;

        public CurrencyFileRepository(IOptions<CurrencyDataOptions> currencyOptions)
        {
            _path = Path.GetFullPath(currencyOptions?.Value?.Path?? throw new ArgumentNullException(nameof(currencyOptions)));
            if (!File.Exists(_path))
            {
                throw new ArgumentException($"{_path} does not exist. ", nameof(currencyOptions.Value.Path));
            }
        }

        public async Task<IEnumerable<Currency>> GetCurrencies()
        {
            var json = await File.ReadAllTextAsync(_path);
            return JsonSerializer.Deserialize<List<Currency>>(json);
        }

        public async Task<Currency>GetCurrency(string symbol)
        {
            var currencies = await GetCurrencies();
            return currencies?.FirstOrDefault(c => c.Symbol.Equals(symbol));
        }
    }
}
