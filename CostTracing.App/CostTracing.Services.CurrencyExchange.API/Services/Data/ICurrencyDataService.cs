using CostTracing.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services.Data
{
    public interface ICurrencyDataService
    {
        Task<IEnumerable<Currency>> GetCurrencyDataAsync();

        Task SetCurrentListAsync(CurrencyList list);

        Task<CurrencyList> GetCurrentListAsync();
    }
}
