using CostTracing.Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CostTracing.Core.Utils;

namespace CostTracing.Backend.CurrencyExchange.API.Services.FixerIO
{
    public class FixerIOService : ICurrencyExchangeService
    {
        protected string _apiKey;
        protected CurrencyList _currentList;
        protected IRestClient _restClient = new RestClient("http://data.fixer.io/api");

        public FixerIOService(string apiKey)
        {
            _apiKey = string.IsNullOrEmpty(apiKey)? throw new ArgumentException("Null or empty", nameof(apiKey)) : apiKey;
        }

        protected async Task FetchCurrentListIfOutdatedAsync()
        {
            if (_currentList == null || (_currentList.Date.Day != DateTime.Now.Day && DateTime.Now.ToUniversalTime().Hour < 9))
            {

                var request = new RestRequest("current", Method.GET, DataFormat.Json);
                request.AddQueryParameter("access_key", _apiKey);
                _currentList = await _restClient.ExecuteRequestAsyncThrowEx<CurrencyList>(request);
            }

        }

        public async Task<IEnumerable<Currency>> GetAvailableCurrenciesAsync()
        {
            await FetchCurrentListIfOutdatedAsync();
            return _currentList.Rates.Keys
                .Select(k => new Currency()
                {
                    Symbol = k,
                    Uri = "/currency/" + k
                });
        }

        public async Task<double> ConvertCurrencyAsync(double amount, string baseCurrency, string targetCurrency = "")
        {           
            if (amount == 0)
            {
                return 0;
            }
            await FetchCurrentListIfOutdatedAsync();
            baseCurrency = string.IsNullOrEmpty(baseCurrency) ? _currentList.Base : baseCurrency;
            targetCurrency = string.IsNullOrEmpty(targetCurrency) ? _currentList.Base : targetCurrency;
            if (baseCurrency.Equals(targetCurrency))
            {
                return amount;
            }
            if (!(_currentList.Rates.ContainsKey(baseCurrency) || _currentList.Base.Equals(baseCurrency)))
            {
                throw new ArgumentException($"No exchange data for {baseCurrency} available", nameof(baseCurrency));
            }
            if (!(_currentList.Rates.ContainsKey(targetCurrency) || _currentList.Base.Equals(targetCurrency)))
            {
                throw new ArgumentException($"No exchange data for {targetCurrency} available", nameof(targetCurrency));
            }

            var baseToListBaseFactor = _currentList.Base.Equals(baseCurrency)? 1.0 : _currentList.Rates[baseCurrency];
            var targetToListBaseFactor = _currentList.Base.Equals(targetCurrency) ? 1.0 : _currentList.Rates[targetCurrency];

            var listBaseAmount = amount / baseToListBaseFactor;
            return listBaseAmount * targetToListBaseFactor;
        }
    }
}
