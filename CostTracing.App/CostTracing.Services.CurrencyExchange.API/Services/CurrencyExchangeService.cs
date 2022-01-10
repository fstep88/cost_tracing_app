using CostTracing.Backend.CurrencyExchange.API.Services.Data;
using CostTracing.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services
{
    public class CurrencyExchangeService : ICurrencyExchangeService
    {

        protected ICurrencyRepository _currencyRepository;
        protected ICurrencyExchangeDataRepository _exchangeDataRepository;

        public CurrencyExchangeService(ICurrencyRepository currencyRepository, ICurrencyExchangeDataRepository exchangeDataRepository)
        {
            _currencyRepository = currencyRepository ?? throw new ArgumentNullException(nameof(currencyRepository));
            _exchangeDataRepository = exchangeDataRepository ?? throw new ArgumentNullException(nameof(exchangeDataRepository));

        }

        public async Task<IEnumerable<CurrencyConversionResult>> ConvertCurrencyAsync(double amount, string baseCurrency, params string[] targetCurrencySymbols)
        {
            var currentExchangeRates = await _exchangeDataRepository.GetCurrencyExchangeData();

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

        public async Task<IEnumerable<Currency>> GetAvailableCurrenciesAsync()
        {
            var currentAvailableList = await _exchangeDataRepository.GetCurrencyExchangeData();
            var list = await _currencyRepository.GetCurrencies();
            return list.Where(currency => currentAvailableList.Rates.ContainsKey(currency.Symbol));
        }
    }
}
