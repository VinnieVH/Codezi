using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Codezi.Domain.Configuration;
using Codezi.Domain.Models;
using Codezi.Domain.Services;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Codezi.Application.Services
{
    public class StockService : IStockService
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly SecretSettings _settings;

        public StockService(IOptions<SecretSettings> settings)
        {
            _httpClient.BaseAddress = new Uri("https://cloud.iexapis.com/");
            _httpClient.DefaultRequestHeaders.Clear();
            _settings = settings.Value;
        }

        public async Task<Company> GetCompanyBySymbolAsync(string symbol)
        {
            var request = new HttpRequestMessage(
                HttpMethod.Get,
                $"stable/stock/{symbol}/company?token={_settings.ApiToken}");
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
