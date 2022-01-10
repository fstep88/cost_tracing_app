using CostTracing.Core.Models;
using CostTracing.Core.Utils;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CostTracing.Backend.CurrencyExchange.API.Services.Data
{
    public class FixerIOService : ICurrencyExchangeDataRepository
    {

        protected string _path;
        protected string _apiKey;
        protected IRestClient _restClient = new RestClient("http://data.fixer.io/api");

        public async Task<CurrencyList> GetCurrentListAsync()
        {
            if (!File.Exists(_path))
            {
                return null;
            }

            using (var fs = new FileStream(_path, FileMode.Open))
            {
                return await System.Text.Json.JsonSerializer.DeserializeAsync<CurrencyList>(fs);
            }
        }

        public async Task SetCurrentListAsync(CurrencyList list)
        {
            await File.WriteAllTextAsync(_path, System.Text.Json.JsonSerializer.Serialize(list, typeof(CurrencyList)));
        }

        public async Task<CurrencyList> GetCurrencyExchangeData()
        {
            var list = await GetCurrentListAsync();
            var currentDate = DateTime.Now.ToUniversalTime();
            var listDate = list !=null? list.Date.ToUniversalTime() : DateTime.Now.ToUniversalTime();

            if (list == null || currentDate - listDate <= TimeSpan.FromHours(8))
            {

                var request = new RestRequest("current", Method.GET, DataFormat.Json);
                request.AddQueryParameter("access_key", _apiKey);
                list = await _restClient.ExecuteRequestAsyncThrowEx<CurrencyList>(request);
                await SetCurrentListAsync(list);
            }

            return list;
        }
    }
}
