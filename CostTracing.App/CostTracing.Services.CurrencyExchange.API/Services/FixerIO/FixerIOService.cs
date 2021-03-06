using CostTracing.Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostTracing.Core.Utils;
using CostTracing.Backend.CurrencyExchange.API.Services.Data;

namespace CostTracing.Backend.CurrencyExchange.API.Services.FixerIO
{
    public class FixerIOService : ICurrencyExchangeService
    {
        protected string _apiKey;
        protected IRestClient _restClient = new RestClient("http://data.fixer.io/api");

        public FixerIOService(string apiKey)
        {
            _apiKey = string.IsNullOrEmpty(apiKey)? throw new ArgumentException("Null or empty", nameof(apiKey)) : apiKey;
        }

        protected async Task<CurrencyList> FetchCurrentListIfOutdatedAsync(ICurrencyDataService dataService)
        {
            var list = await dataService.GetCurrentListAsync();

            if (list == null || (list.Date.Day != DateTime.Now.Day && DateTime.Now.ToUniversalTime().Hour < 9))
            {

                var request = new RestRequest("current", Method.GET, DataFormat.Json);
                request.AddQueryParameter("access_key", _apiKey);
                list = await _restClient.ExecuteRequestAsyncThrowEx<CurrencyList>(request);
                await dataService.SetCurrentListAsync(list);
            }

            return list;
        }

        public async Task<IEnumerable<Currency>> GetAvailableCurrenciesAsync(ICurrencyDataService dataService)
        {
            var currentAvailableList = await FetchCurrentListIfOutdatedAsync(dataService);
            var list = await dataService.GetCurrencyDataAsync();
            return list.Where(currency => currentAvailableList.Rates.ContainsKey(currency.Symbol));
        }

        public async Task<IEnumerable<CurrencyConversionResult>> ConvertCurrencyAsync(ICurrencyDataService dataService, double amount, string baseCurrency, params string[] targetCurrencySymbols)
        {
            var currentExchangeRates = await FetchCurrentListIfOutdatedAsync(dataService);

            if (!(currentExchangeRates.Rates.ContainsKey(baseCurrency) || currentExchangeRates.Base.Equals(baseCurrency)))
            {
                throw new ArgumentException($"No exchange data for {baseCurrency} available", nameof(baseCurrency));
            }

            return targetCurrencySymbols?.Distinct()
                .Where(cSymbol => currentExchangeRates.Rates.ContainsKey(cSymbol))
                .Select(c =>
                new CurrencyConversionResult()
                {
                    TargetSymbol = c,
                    Amount = Convert(amount, baseCurrency, c, currentExchangeRates)
                });

        }


        protected double Convert(double amount, string baseCurrency, string targetCurrency, CurrencyList currentExchangeRates)
        {
            if (amount == 0)
                return 0;
            if (baseCurrency.Equals(targetCurrency))
            {
                return amount;
            }
            if (!(currentExchangeRates.Rates.ContainsKey(baseCurrency) || currentExchangeRates.Base.Equals(baseCurrency)))
            {
                throw new ArgumentException($"No exchange data for {baseCurrency} available", nameof(baseCurrency));
            }
            if (!(currentExchangeRates.Rates.ContainsKey(targetCurrency) || currentExchangeRates.Base.Equals(targetCurrency)))
            {
                throw new ArgumentException($"No exchange data for {targetCurrency} available", nameof(targetCurrency));
            }

            var baseToListBaseFactor = currentExchangeRates.Base.Equals(baseCurrency) ? 1.0 : currentExchangeRates.Rates[baseCurrency];
            var targetToListBaseFactor = currentExchangeRates.Base.Equals(targetCurrency) ? 1.0 : currentExchangeRates.Rates[targetCurrency];

            var listBaseAmount = amount / baseToListBaseFactor;
            return listBaseAmount * targetToListBaseFactor;
        }
    }
}
