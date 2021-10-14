using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace CostTracing.Core.Utils
{
    public static class RestSharpExtensions
    {
        public static async Task<string> ExecuteRequestAsyncThrowEx(this IRestClient client, IRestRequest request)
        {
            var response = await client.ExecuteAsync(request);
            response.ValidateAndThrowExOnFailure(request);
            return response.Content;
        }
        

        public static void ValidateAndThrowExOnFailure(this IRestResponse response, IRestRequest request)
        {
            if (!response.IsSuccessful)
            {
                throw new Exception($"Executing {request.Method} to {request.Resource} returned {response.StatusCode}: {response.ErrorMessage}{response.ErrorException}{response.Content}");
            }
        }

        public static async Task<T> ExecuteRequestAsyncThrowEx<T>(this IRestClient client, IRestRequest request)
        {
            var response = await client.ExecuteAsync(request);
            response.ValidateAndThrowExOnFailure(request);
            return client.Deserialize<T>(response).Data;
        }
    }
}
