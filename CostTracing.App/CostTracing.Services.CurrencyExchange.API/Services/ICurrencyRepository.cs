using CostTracing.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services
{
    public interface ICurrencyRepository
    {
        Task<IEnumerable<Currency>> GetCurrencies();

        Task<Currency> GetCurrency(string symbol);
    }
}
