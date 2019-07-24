using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codezi.Domain.Models;
using Codezi.Domain.Services;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Codezi.Application.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly string _apiKey;

        public StockService(IConfiguration configuration)
        {
            // IEX base address: https://cloud.iexapis.com/
            // AlphaVantage base address: https://www.alphavantage.co/
            _httpClient.BaseAddress = new Uri("https://www.alphavantage.co/");
            _httpClient.DefaultRequestHeaders.Clear();
            // AVToken || IEXToken
            _apiKey = configuration.GetSection("Secrets:AVToken").Value;
        }

        public async Task<Company> GetCompanyBySymbolAsync(string symbol)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"stable/stock/{symbol}/company?token={_apiKey}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await _httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var jsonSerializer = new JsonSerializer();
                        var company = jsonSerializer.Deserialize<Company>(jsonTextReader);

                        return company;
                    }
                }
            }
        }

        public async Task<Company> GetIntradayByMinuteAsync(string symbol)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"query?function=TIME_SERIES_INTRADAY&symbol={symbol}&interval=1min&apikey={_apiKey}");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            using (var response = await _httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stream = await response.Content.ReadAsStreamAsync();

                using (var streamReader = new StreamReader(stream))
                {
                    using (var jsonTextReader = new JsonTextReader(streamReader))
                    {
                        var jsonSerializer = new JsonSerializer();
                        var company = jsonSerializer.Deserialize<Company>(jsonTextReader);

                        return company;
                    }
                }
            }
        }
    }
}
